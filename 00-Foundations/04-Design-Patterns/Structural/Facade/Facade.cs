// Illustrates notes.md in this folder — the Facade pattern.
//   dotnet run --project Runner facade

namespace Foundations.Patterns.Structural.Facade;

public class InventoryService
{
    public bool Reserve(string sku, int qty)
    {
        Console.WriteLine($"Reserved {qty}x {sku}");
        return true;
    }
}

public class PaymentService
{
    public bool Charge(string customerId, decimal amount)
    {
        Console.WriteLine($"Charged {customerId} {amount:C}");
        return true;
    }
}

public class ShippingService
{
    public void Schedule(string customerId)
    {
        Console.WriteLine($"Shipping scheduled for {customerId}");
    }
}

public class NotificationService
{
    public void ConfirmOrder(string customerId)
    {
        Console.WriteLine($"Confirmation sent to {customerId}");
    }
}

public record CartItem(string Sku, int Quantity, decimal Price);

// Simple front door over four services. Callers who need the individual
// steps can still use InventoryService/PaymentService/... directly — the
// facade is an addition, not a replacement.
public class CheckoutFacade
{
    private readonly InventoryService _inventory = new();
    private readonly PaymentService _payment = new();
    private readonly ShippingService _shipping = new();
    private readonly NotificationService _notification = new();

    public bool PlaceOrder(string customerId, List<CartItem> cart)
    {
        foreach (var item in cart)
        {
            if (!_inventory.Reserve(item.Sku, item.Quantity))
                return false;
        }

        decimal total = cart.Sum(i => i.Price * i.Quantity);
        if (!_payment.Charge(customerId, total))
            return false;

        _shipping.Schedule(customerId);
        _notification.ConfirmOrder(customerId);
        return true;
    }
}

public static class FacadeDemo
{
    public static void Run()
    {
        var facade = new CheckoutFacade();
        var cart = new List<CartItem> { new("SKU-1", 2, 9.99m) };
        facade.PlaceOrder("C-1", cart);
    }
}
