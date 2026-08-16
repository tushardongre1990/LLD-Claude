// Illustrates notes.md in this folder — the Adapter pattern.
//   dotnet run --project Runner adapter

namespace Foundations.Patterns.Structural.Adapter;

// The interface OUR application already codes against.
public interface IPaymentGateway
{
    bool Charge(int amountCents);
}

// A third-party SDK we don't control — different units (dollars, not
// cents), different method name, different return type.
public class LegacyStripeSdk
{
    public string MakePayment(decimal amountDollars)
    {
        Console.WriteLine($"[LegacyStripeSdk] Charged ${amountDollars}");
        return "SUCCESS";
    }
}

// Adapts LegacyStripeSdk to look like an IPaymentGateway. All the
// translation logic (cents->dollars, string result->bool) lives in exactly
// one place.
public class StripeAdapter : IPaymentGateway
{
    private readonly LegacyStripeSdk _sdk;

    public StripeAdapter(LegacyStripeSdk sdk)
    {
        _sdk = sdk;
    }

    public bool Charge(int amountCents)
    {
        decimal dollars = amountCents / 100m;
        string result = _sdk.MakePayment(dollars);
        return result == "SUCCESS";
    }
}

// Application code depends only on IPaymentGateway — swapping payment
// providers later means writing a new adapter, not touching this class.
public class CheckoutService
{
    private readonly IPaymentGateway _gateway;

    public CheckoutService(IPaymentGateway gateway)
    {
        _gateway = gateway;
    }

    public bool Pay(int amountCents) => _gateway.Charge(amountCents);
}

public static class AdapterDemo
{
    public static void Run()
    {
        var checkout = new CheckoutService(new StripeAdapter(new LegacyStripeSdk()));
        checkout.Pay(4999); // $49.99
    }
}
