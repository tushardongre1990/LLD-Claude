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
    public void LockOrdering_BooksAllSeats_OrNone()
    {
        var booking = new SeatBooking();
        var seats = new[] { new SafeSeat("C-3"), new SafeSeat("A-1"), new SafeSeat("B-2") };

        Assert.True(booking.TryBookAll(seats, "alice"));
        Assert.All(seats, s => Assert.Equal("alice", s.BookedBy));

        // A second attempt over the same seats fails cleanly.
        Assert.False(booking.TryBookAll(seats, "bob"));
    }
}
