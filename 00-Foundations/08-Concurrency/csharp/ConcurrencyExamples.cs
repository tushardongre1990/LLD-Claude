// Illustrates notes.md — every section of the concurrency chapter.
//   dotnet run --project Runner concurrency
//
// Read the notes alongside this file: the section numbers below match its
// headings. Several examples are deliberately broken — the class names say
// which (Unsafe*, *DeadlockProne, *Unsafe methods).

using System.Collections.Concurrent;

namespace Foundations.Concurrency;

// ---------------------------------------------------------------------
// 1. THE BUG: check-then-act race condition.
// ---------------------------------------------------------------------
public class UnsafeSeat
{
    private string? _bookedBy;

    public bool IsAvailable => _bookedBy is null;

    public void Book(string userId) => _bookedBy = userId;

    public string? BookedBy => _bookedBy;
}

// ---------------------------------------------------------------------
// 2. THE FIX: make check-and-act one atomic operation, and expose THAT
//    as the API. Note there is deliberately no public IsAvailable that
//    callers could check before calling — if they could, they'd be able
//    to reintroduce the race themselves.
// ---------------------------------------------------------------------
public class SafeSeat
{
    private readonly object _lock = new(); // private: external code can't lock it
    private string? _bookedBy;

    public string Id { get; }

    public SafeSeat(string id) => Id = id;

    public bool TryBook(string userId)
    {
        lock (_lock)
        {
            if (_bookedBy is not null)
                return false;      // already taken

            _bookedBy = userId;    // check and act, indivisibly
            return true;
        }
    }

    // Needed for compensating rollback (see SeatBooking below).
    // Only the current owner may release — otherwise any caller could
    // free someone else's seat, which is a worse bug than the one
    // rollback exists to fix.
    public bool Release(string userId)
    {
        lock (_lock)
        {
            if (_bookedBy != userId)
                return false;

            _bookedBy = null;
            return true;
        }
    }

    public string? BookedBy
    {
        get { lock (_lock) { return _bookedBy; } }
    }
}

// ---------------------------------------------------------------------
// 3. OPTIMISTIC CONCURRENCY: no locking; detect conflicts via a version
//    that must match at write time. This is what a DB row-version /
//    ETag does, modelled in memory with Interlocked.CompareExchange.
//
//    ⚠️ SCOPE: this class is a deliberately simplified ONE-SHOT CLAIM.
//    It works because a seat only ever goes unbooked -> booked, once.
//    `_version` and `_bookedBy` are separate fields, so the version
//    check and the state change are NOT one atomic step — that is fine
//    here only because the CAS on _bookedBy is itself the thing that
//    decides the winner. Add a second mutable field (price, status,
//    cancellation) and this shape breaks down.
//
//    For the general model, see VersionedSeatState below.
// ---------------------------------------------------------------------
public class VersionedSeat
{
    private int _version;
    private string? _bookedBy;

    public int Version => Volatile.Read(ref _version);

    public string? BookedBy => Volatile.Read(ref _bookedBy);

    public bool TryBook(string userId, int expectedVersion)
    {
        // (a) The OPTIMISTIC check — did anything change since we read?
        //     This is what rejects a write built on stale data.
        if (Volatile.Read(ref _version) != expectedVersion)
            return false;

        // (b) The DOMAIN invariant — a seat is bookable at most once.
        //     This MUST be a single atomic compare-and-swap, not a
        //     read-then-write. Exactly one thread can flip null -> userId;
        //     everyone else sees a non-null value and loses.
        //
        //     Writing this as `if (_bookedBy is null) _bookedBy = userId;`
        //     reintroduces check-then-act: a second thread can read null
        //     in the gap before the first thread's write lands, and both
        //     "win". That bug is subtle enough to survive a passing test
        //     and only show up under real load.
        if (Interlocked.CompareExchange(ref _bookedBy, userId, null) is not null)
            return false;

        Interlocked.Increment(ref _version);
        return true;
    }
}

// ---------------------------------------------------------------------
// 3b. GENERAL OPTIMISTIC CONCURRENCY.
//
//     The fix for VersionedSeat's limitation: put ALL mutable state in
//     one immutable object, and swap the whole reference atomically.
//     Now "check the version" and "write the new state" really are a
//     single indivisible operation, and it scales to any number of
//     fields and any transition — not just a one-shot claim.
//
//     This is the in-memory equivalent of
//        UPDATE seats SET ... WHERE id = @id AND version = @version
// ---------------------------------------------------------------------
public sealed record SeatState(string? BookedBy, decimal Price, int Version);

public class VersionedSeatState
{
    private SeatState _state = new(BookedBy: null, Price: 250m, Version: 0);

    public SeatState Current => Volatile.Read(ref _state);

    // Callers read Current, compute the next state from it, and try to
    // commit. If anyone else committed in between, this returns false
    // and the caller retries against the fresh state.
    public bool TryUpdate(SeatState expected, SeatState next)
    {
        if (next.Version != expected.Version + 1)
            throw new ArgumentException("next must increment the version.");

        // One atomic swap of the ENTIRE state. No window, no torn reads.
        return ReferenceEquals(
            Interlocked.CompareExchange(ref _state, next, expected),
            expected);
    }

    // Typical retry loop built on top of TryUpdate.
    public bool TryBook(string userId, int maxAttempts = 10)
    {
        for (int attempt = 0; attempt < maxAttempts; attempt++)
        {
            SeatState current = Current;

            if (current.BookedBy is not null)
                return false; // domain rule: already taken, retrying won't help

            var next = current with { BookedBy = userId, Version = current.Version + 1 };

            if (TryUpdate(current, next))
                return true;

            // Lost the race — loop and re-read. This is the "optimistic"
            // bet: conflicts are assumed rare enough that retrying beats
            // blocking every caller with a lock.
        }
        return false;
    }
}

// ---------------------------------------------------------------------
// 4. MULTI-RESOURCE BOOKING via compensating rollback.
//
//    IMPORTANT — read the caveat, it is the actual lesson here:
//    this is NOT atomic. Each seat is claimed under its OWN lock, one
//    after another, so between the first claim and the rollback there is
//    a window where another thread can observe seats this caller is
//    about to give back. That is a "compensating transaction": it
//    restores the end state, it does not hide the intermediate one.
//
//    Real booking systems avoid the window with a Held state + timeout
//    (notes.md §6) or a database transaction. For the genuinely
//    simultaneous multi-lock case, see LockOrdering below.
// ---------------------------------------------------------------------
public class SeatBooking
{
    public bool TryBookAll(IEnumerable<SafeSeat> seats, string userId)
    {
        // Deterministic order keeps behaviour reproducible and matches
        // the order a simultaneous-lock implementation would need.
        var ordered = seats.OrderBy(s => s.Id, StringComparer.Ordinal).ToList();

        var acquired = new List<SafeSeat>();
        foreach (var seat in ordered)
        {
            if (seat.TryBook(userId))
            {
                acquired.Add(seat);
                continue;
            }

            // Actually give back what we already took — not just log it.
            foreach (var taken in acquired)
                taken.Release(userId);

            return false;
        }
        return true;
    }
}

// ---------------------------------------------------------------------
// 5. DEADLOCK AND LOCK ORDERING — the real thing.
//
//    Lock ordering only matters when a thread holds SEVERAL locks at
//    once. The seat code above never does (each lock is released before
//    the next is taken), so it cannot deadlock — and equally, it cannot
//    demonstrate the fix. Account transfer is the classic case that can.
// ---------------------------------------------------------------------
public class Account
{
    internal readonly object Lock = new();
    private decimal _balance;

    public string Id { get; }

    public Account(string id, decimal openingBalance)
    {
        Id = id;
        _balance = openingBalance;
    }

    public decimal Balance
    {
        get { lock (Lock) { return _balance; } }
    }

    // Callers MUST already hold this account's lock.
    internal bool WithdrawUnsafe(decimal amount)
    {
        if (_balance < amount) return false;
        _balance -= amount;
        return true;
    }

    internal void DepositUnsafe(decimal amount) => _balance += amount;
}

public static class LockOrdering
{
    // DEADLOCK-PRONE: locks in the order the arguments happen to arrive.
    // Thread 1 doing Transfer(A, B) and thread 2 doing Transfer(B, A) can
    // each hold one lock and wait forever for the other.
    public static bool TransferDeadlockProne(Account from, Account to, decimal amount)
    {
        lock (from.Lock)
        {
            lock (to.Lock)
            {
                if (!from.WithdrawUnsafe(amount)) return false;
                to.DepositUnsafe(amount);
                return true;
            }
        }
    }

    // SAFE: both locks are still held simultaneously — that is required
    // for the transfer to be atomic — but they are always acquired in a
    // globally consistent order (by Id). With every thread agreeing on
    // the order, no wait cycle can form, so deadlock is impossible.
    public static bool Transfer(Account from, Account to, decimal amount)
    {
        if (ReferenceEquals(from, to)) return false;

        var (first, second) = string.CompareOrdinal(from.Id, to.Id) < 0
            ? (from, to)
            : (to, from);

        lock (first.Lock)
        {
            lock (second.Lock)   // nested: both held at the same time
            {
                if (!from.WithdrawUnsafe(amount)) return false;
                to.DepositUnsafe(amount);
                return true;
            }
        }
    }
}

// ---------------------------------------------------------------------
// 5. CONCURRENT COLLECTIONS: atomic per-operation, NOT per-compound-logic.
// ---------------------------------------------------------------------
public class TicketRegistry
{
    private readonly ConcurrentDictionary<string, string> _tickets = new();

    // WRONG: ContainsKey + indexer is still a check-then-act race, even
    // though the collection itself is "thread-safe".
    public void RegisterUnsafe(string id, string owner)
    {
        if (!_tickets.ContainsKey(id))
            _tickets[id] = owner;
    }

    // RIGHT: one atomic operation the collection guarantees for you.
    public bool Register(string id, string owner) => _tickets.TryAdd(id, owner);

    public int Count => _tickets.Count;
}

public static class ConcurrencyDemo
{
    public static void Run()
    {
        // Demonstrate the race — and, just as importantly, how RARELY it
        // fires. A single run almost always looks fine: the first thread
        // sets _bookedBy within nanoseconds, so the check-then-act window
        // is tiny. Repeat the experiment to prove the bug is really there.
        //
        // This intermittency IS the lesson. A one-shot run, or a unit test
        // at low contention, would have reported success and taught you
        // nothing. Never conclude "no race" from a green test.
        const int trials = 200;
        int trialsWithMultipleWinners = 0;

        for (int t = 0; t < trials; t++)
        {
            var unsafeSeat = new UnsafeSeat();
            int believedTheyWon = 0;

            Parallel.For(0, 1000, i =>
            {
                if (unsafeSeat.IsAvailable)
                {
                    Interlocked.Increment(ref believedTheyWon);
                    unsafeSeat.Book($"user-{i}");
                }
            });

            if (believedTheyWon > 1)
                trialsWithMultipleWinners++;
        }

        Console.WriteLine(
            $"Unsafe seat: {trialsWithMultipleWinners}/{trials} trials handed the " +
            "same seat to more than one user.");
        Console.WriteLine(
            "  (Usually a low number — the race is real but intermittent, which is " +
            "exactly why check-then-act bugs survive testing and surface in prod.)");

        // The safe version: exactly one winner, guaranteed.
        var safeSeat = new SafeSeat("A-42");
        int winners = 0;
        Parallel.For(0, 1000, i =>
        {
            if (safeSeat.TryBook($"user-{i}"))
                Interlocked.Increment(ref winners);
        });
        Console.WriteLine($"Safe seat winners: {winners} (always exactly 1)");

        // Optimistic: losers detect the conflict and can retry.
        var versioned = new VersionedSeat();
        int optimisticWinners = 0;
        Parallel.For(0, 1000, i =>
        {
            int v = versioned.Version;
            if (versioned.TryBook($"user-{i}", v))
                Interlocked.Increment(ref optimisticWinners);
        });
        Console.WriteLine($"Optimistic winners: {optimisticWinners}");

        // General OCC with whole-state CAS + retry loop.
        var stateSeat = new VersionedSeatState();
        int occWinners = 0;
        Parallel.For(0, 1000, i =>
        {
            if (stateSeat.TryBook($"user-{i}"))
                Interlocked.Increment(ref occWinners);
        });
        Console.WriteLine($"Whole-state OCC winners: {occWinners}, " +
                          $"final version {stateSeat.Current.Version}");

        // Multi-seat booking: partial failure must give back what it took.
        var booking = new SeatBooking();
        var free = new SafeSeat("A-1");
        var alsoFree = new SafeSeat("B-2");
        var taken = new SafeSeat("C-3");
        taken.TryBook("someone-else");

        bool ok = booking.TryBookAll(new[] { free, alsoFree, taken }, "alice");
        Console.WriteLine($"TryBookAll succeeded: {ok}; " +
                          $"A-1 owner: {free.BookedBy ?? "<released>"}, " +
                          $"B-2 owner: {alsoFree.BookedBy ?? "<released>"}");

        // Concurrent collection: TryAdd is atomic, so exactly one caller
        // wins the same key. Switch this to RegisterUnsafe and the race
        // becomes INVISIBLE — still one entry, but the losers silently
        // overwrote each other and nothing can report a winner.
        var registry = new TicketRegistry();
        int registered = 0;
        Parallel.For(0, 500, i =>
        {
            if (registry.Register("T-1", $"user-{i}"))
                Interlocked.Increment(ref registered);
        });
        Console.WriteLine($"TryAdd winners: {registered} (always exactly 1), " +
                          $"entries: {registry.Count}");

        // Deadlock avoidance: consistent lock ordering under contention.
        var acc1 = new Account("ACC-1", 1000m);
        var acc2 = new Account("ACC-2", 1000m);
        Parallel.For(0, 2000, i =>
        {
            // Opposing directions — would deadlock without ordered locks.
            if (i % 2 == 0) LockOrdering.Transfer(acc1, acc2, 1m);
            else LockOrdering.Transfer(acc2, acc1, 1m);
        });
        Console.WriteLine($"Transfers completed without deadlock. " +
                          $"Total preserved: {acc1.Balance + acc2.Balance:C}");
    }
}
