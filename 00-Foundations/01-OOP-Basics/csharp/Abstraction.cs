// Illustrates notes.md §3.2 — Abstraction.
//   dotnet run --project Runner abstraction

namespace Foundations.OopBasics;

// Abstraction: callers depend only on the interface, never on which payment
// method is actually used underneath.
public interface IPaymentProcessor
{
    bool Pay(decimal amount);
}

public class CreditCardProcessor : IPaymentProcessor
{
    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Charged {amount:C} to credit card.");
        return true;
    }
}

public class UpiProcessor : IPaymentProcessor
{
    public bool Pay(decimal amount)
    {
        Console.WriteLine($"Charged {amount:C} via UPI.");
        return true;
    }
}

// This class knows nothing about credit cards or UPI — only the contract.
// Add a new IPaymentProcessor implementation later and Checkout is unchanged.
public class Checkout
{
    private readonly IPaymentProcessor _processor;

    public Checkout(IPaymentProcessor processor)
    {
        _processor = processor;
    }

    public bool CompleteOrder(decimal total) => _processor.Pay(total);
}

public static class AbstractionDemo
{
    public static void Run()
    {
        var checkout = new Checkout(new CreditCardProcessor());
        checkout.CompleteOrder(499.00m);
    }
}
