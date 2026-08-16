namespace Foundations.Solid.Srp.Violation
{
    // Three reasons to change live in one class: billing rules, persistence
    // technology, and receipt formatting.
    public class ParkingTicketManager
    {
        public decimal CalculateFee(int hours) => 20m + 10m * hours;

        public void SaveToDatabase(string ticketId)
        {
            Console.WriteLine($"INSERT INTO tickets ... {ticketId}");
        }

        public void PrintReceipt(string ticketId, decimal fee)
        {
            Console.WriteLine($"Receipt [{ticketId}]: {fee:C}");
        }
    }
}

namespace Foundations.Solid.Srp.Fixed
{
    // Each class now changes for exactly one reason.
    public class FeeCalculator
    {
        public decimal CalculateFee(int hours) => 20m + 10m * hours;
    }

    public class ParkingRepository
    {
        public void Save(string ticketId)
        {
            Console.WriteLine($"INSERT INTO tickets ... {ticketId}");
        }
    }

    public class ReceiptPrinter
    {
        public void Print(string ticketId, decimal fee)
        {
            Console.WriteLine($"Receipt [{ticketId}]: {fee:C}");
        }
    }

    public static class SrpDemo
    {
        public static void Run()
        {
            var calculator = new FeeCalculator();
            var repository = new ParkingRepository();
            var printer = new ReceiptPrinter();

            decimal fee = calculator.CalculateFee(3);
            repository.Save("T-100");
            printer.Print("T-100", fee);
        }
    }
}
