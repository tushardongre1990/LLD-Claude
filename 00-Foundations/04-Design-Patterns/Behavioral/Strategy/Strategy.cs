// Illustrates notes.md in this folder — the Strategy pattern.
//   dotnet run --project Runner strategy

namespace Foundations.Patterns.Behavioral.Strategy;

public interface IFeeStrategy
{
    decimal Calculate(int hours);
}

public class HourlyFeeStrategy : IFeeStrategy
{
    public decimal Calculate(int hours) => 10m * hours;
}

public class FlatDayRateStrategy : IFeeStrategy
{
    public decimal Calculate(int hours) => 50m; // capped, regardless of hours
}

public class FreeFirstHourStrategy : IFeeStrategy
{
    public decimal Calculate(int hours) => hours <= 1 ? 0m : 10m * (hours - 1);
}

// Depends only on the interface. Swapping pricing schemes never requires
// editing this class — new strategies are added, not this class modified.
public class ParkingTicket
{
    private readonly IFeeStrategy _strategy;

    public ParkingTicket(IFeeStrategy strategy)
    {
        _strategy = strategy;
    }

    public decimal CalculateFee(int hours) => _strategy.Calculate(hours);
}

public static class StrategyDemo
{
    public static void Run()
    {
        var promoTicket = new ParkingTicket(new FreeFirstHourStrategy());
        Console.WriteLine(promoTicket.CalculateFee(3)); // 20

        var regularTicket = new ParkingTicket(new HourlyFeeStrategy());
        Console.WriteLine(regularTicket.CalculateFee(3)); // 30
    }
}
