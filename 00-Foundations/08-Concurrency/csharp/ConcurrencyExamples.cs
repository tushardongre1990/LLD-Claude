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

    public string? BookedBy
    {
        get { lock (_lock) { return _bookedBy; } }
    }
}

// ---------------------------------------------------------------------
// 3. OPTIMISTIC CONCURRENCY: no locking; detect conflicts via a version
//    that must match at write time. This is what a DB row-version /
//    ETag does, modelled in memory with Interlocked.CompareExchange.
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
// 4. DEADLOCK AVOIDANCE via consistent lock ordering, for booking
//    several seats at once.
// ---------------------------------------------------------------------
public class SeatBooking
{
    public bool TryBookAll(IEnumerable<SafeSeat> seats, string userId)
    {
        // Always acquire in the same global order (by Id) so two threads
        // booking overlapping seat sets can never form a wait cycle.
        var ordered = seats.OrderBy(s => s.Id, StringComparer.Ordinal).ToList();

        var acquired = new List<SafeSeat>();
        foreach (var seat in ordered)
        {
            if (!seat.TryBook(userId))
            {
                // All-or-nothing: roll back what we already took.
                // (A real system would use a transaction or a Held state
                //  with a timeout — see notes.md §6.)
                foreach (var taken in acquired)
                    Console.WriteLine($"Rolling back {taken.Id}");
                return false;
            }
            acquired.Add(seat);
        }
        return true;
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
        // Demonstrate the race: many threads hammering an unsafe seat.
        var unsafeSeat = new UnsafeSeat();
        Parallel.For(0, 1000, i =>
        {
            if (unsafeSeat.IsAvailable)
                unsafeSeat.Book($"user-{i}");
        });
        Console.WriteLine($"Unsafe seat ended up owned by: {unsafeSeat.BookedBy} " +
                          "(many threads believed they had won)");

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
    }
}
