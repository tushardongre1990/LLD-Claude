namespace Foundations.Solid.Dip.Violation
{
    // High-level ParkingLot is welded to a concrete, low-level SqlDatabase.
    // Can't unit test ParkingLot without a real database; can't swap storage.
    public class SqlDatabase
    {
        public void Save(string ticketId) => Console.WriteLine($"SQL INSERT {ticketId}");
    }

    public class ParkingLot
    {
        private readonly SqlDatabase _db = new(); // concrete dependency, constructed internally

        public void IssueTicket(string ticketId) => _db.Save(ticketId);
    }
}

namespace Foundations.Solid.Dip.Fixed
{
    // Both the high-level policy (ParkingLot) and the low-level detail
    // (SqlRepository) depend on this abstraction instead of on each other.
    public interface ITicketRepository
    {
        void Save(string ticketId);
    }

    public class SqlRepository : ITicketRepository
    {
        public void Save(string ticketId) => Console.WriteLine($"SQL INSERT {ticketId}");
    }

    public class InMemoryRepository : ITicketRepository
    {
        private readonly List<string> _tickets = new();
        public void Save(string ticketId) => _tickets.Add(ticketId);
    }

    public class ParkingLot
    {
        private readonly ITicketRepository _repository;

        // Dependency Injection: the concrete implementation is handed in,
        // not constructed internally. This is the mechanism; DIP is the goal.
        public ParkingLot(ITicketRepository repository)
        {
            _repository = repository;
        }

        public void IssueTicket(string ticketId) => _repository.Save(ticketId);
    }

    public static class DipDemo
    {
        public static void Run()
        {
            var productionLot = new ParkingLot(new SqlRepository());
            productionLot.IssueTicket("T-1");

            var testLot = new ParkingLot(new InMemoryRepository()); // fully unit-testable, no real DB
            testLot.IssueTicket("T-2");
        }
    }
}
