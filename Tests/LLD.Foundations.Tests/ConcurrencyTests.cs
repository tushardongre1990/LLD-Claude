using Foundations.Concurrency;

namespace LLD.Foundations.Tests;

// Concurrency tests are probabilistic, not proofs — a race may not
// reproduce on every run. They're still worth writing: they demonstrate
// the invariant, and the "safe" assertions hold deterministically.
// See 00-Foundations/08-Concurrency/notes.md.

public class ConcurrencyTests
{
    [Fact]
    public void SafeSeat_AllowsExactlyOneWinner_UnderContention()
    {
        var seat = new SafeSeat("A-42");
        int winners = 0;

        Parallel.For(0, 1_000, i =>
        {
            if (seat.TryBook($"user-{i}"))
                Interlocked.Increment(ref winners);
        });

        // The invariant: one seat, one owner. Deterministically true.
        Assert.Equal(1, winners);
        Assert.NotNull(seat.BookedBy);
    }

    [Fact]
    public void SafeSeat_SecondBooking_IsRejected()
    {
        var seat = new SafeSeat("A-1");

        Assert.True(seat.TryBook("alice"));
        Assert.False(seat.TryBook("bob"));
        Assert.Equal("alice", seat.BookedBy);
    }

    [Fact]
    public void OptimisticSeat_RejectsWritesBuiltOnAStaleVersion()
    {
        var seat = new VersionedSeat();
        int staleVersion = seat.Version;

        // First writer wins and bumps the version.
        Assert.True(seat.TryBook("alice", staleVersion));

        // Second writer read the same version earlier — its write is
        // detected as a conflict rather than silently overwriting.
        Assert.False(seat.TryBook("bob", staleVersion));
    }

    [Fact]
    public void OptimisticSeat_StillEnforcesTheDomainInvariant()
    {
        var seat = new VersionedSeat();
        Assert.True(seat.TryBook("alice", seat.Version));

        // Even with a perfectly fresh version, the seat is already taken.
        // Optimistic concurrency control prevents lost updates; it does
        // NOT by itself enforce "a seat is bookable at most once".
        Assert.False(seat.TryBook("bob", seat.Version));
        Assert.Equal("alice", seat.BookedBy);
    }

    [Fact]
    public void OptimisticSeat_AllowsExactlyOneWinner_UnderContention()
    {
        var seat = new VersionedSeat();
        int winners = 0;

        Parallel.For(0, 1_000, i =>
        {
            int version = seat.Version;
            if (seat.TryBook($"user-{i}", version))
                Interlocked.Increment(ref winners);
        });

        Assert.Equal(1, winners);
    }

    [Fact]
    public void WholeStateOcc_AllowsExactlyOneWinner_UnderContention()
    {
        var seat = new VersionedSeatState();
        int winners = 0;

        Parallel.For(0, 1_000, i =>
        {
            if (seat.TryBook($"user-{i}"))
                Interlocked.Increment(ref winners);
        });

        Assert.Equal(1, winners);
        Assert.Equal(1, seat.Current.Version); // exactly one committed change
    }

    [Fact]
    public void WholeStateOcc_RejectsCommitBuiltOnStaleState()
    {
        var seat = new VersionedSeatState();
        SeatState stale = seat.Current;

        Assert.True(seat.TryBook("alice"));

        // Committing against the state we read earlier must fail — some-
        // one else advanced it. This is the lost-update prevention.
        var next = stale with { Price = 999m, Version = stale.Version + 1 };
        Assert.False(seat.TryUpdate(stale, next));
        Assert.Equal(250m, seat.Current.Price);
    }

    [Fact]
    public void LockOrdering_PreservesTotalBalance_UnderOpposingTransfers()
    {
        var a = new Account("ACC-1", 1_000m);
        var b = new Account("ACC-2", 1_000m);

        // Opposing directions: the deadlock-prone version could hang here.
        // The ordered version completes and conserves the total.
        Parallel.For(0, 2_000, i =>
        {
            if (i % 2 == 0) LockOrdering.Transfer(a, b, 1m);
            else LockOrdering.Transfer(b, a, 1m);
        });

        Assert.Equal(2_000m, a.Balance + b.Balance);
    }

    [Fact]
    public void Transfer_FailsAndChangesNothing_WhenFundsAreInsufficient()
    {
        var a = new Account("ACC-1", 10m);
        var b = new Account("ACC-2", 0m);

        Assert.False(LockOrdering.Transfer(a, b, 100m));
        Assert.Equal(10m, a.Balance);
        Assert.Equal(0m, b.Balance);
    }

    [Fact]
    public void ConcurrentDictionary_TryAdd_IsAtomic()
    {
        var registry = new TicketRegistry();
        int successes = 0;

        Parallel.For(0, 500, i =>
        {
            if (registry.Register("T-1", $"user-{i}"))
                Interlocked.Increment(ref successes);
        });

        Assert.Equal(1, successes);
        Assert.Equal(1, registry.Count);
    }

    [Fact]
    public void TryBookAll_BooksEverySeat_WhenAllAreFree()
    {
        var booking = new SeatBooking();
        var seats = new[] { new SafeSeat("C-3"), new SafeSeat("A-1"), new SafeSeat("B-2") };

        Assert.True(booking.TryBookAll(seats, "alice"));
        Assert.All(seats, s => Assert.Equal("alice", s.BookedBy));
    }

    // Regression test for a real bug: the rollback path used to only LOG
    // "Rolling back X" without releasing anything, so a partial failure
    // left the caller owning seats while reporting failure. The original
    // test missed it because it never hit the partial-failure path — the
    // second call failed on the FIRST seat, so nothing needed rolling back.
    [Fact]
    public void TryBookAll_ReleasesSeatsAlreadyTaken_WhenALaterSeatFails()
    {
        var booking = new SeatBooking();
        var free = new SafeSeat("A-1");
        var alsoFree = new SafeSeat("B-2");
        var contested = new SafeSeat("C-3");

        contested.TryBook("bob"); // C-3 is gone before alice tries

        bool result = booking.TryBookAll(new[] { free, alsoFree, contested }, "alice");

        Assert.False(result);
        // The point: alice must not still be holding A-1 and B-2.
        Assert.Null(free.BookedBy);
        Assert.Null(alsoFree.BookedBy);
        Assert.Equal("bob", contested.BookedBy); // untouched
    }

    [Fact]
    public void Release_OnlySucceeds_ForTheCurrentOwner()
    {
        var seat = new SafeSeat("A-1");
        seat.TryBook("alice");

        Assert.False(seat.Release("bob"));    // not yours
        Assert.Equal("alice", seat.BookedBy);

        Assert.True(seat.Release("alice"));
        Assert.Null(seat.BookedBy);
    }
}
