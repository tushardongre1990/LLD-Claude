namespace Foundations.Patterns.Behavioral.State;

public interface IOrderState
{
    void Pay(Order order);
    void Ship(Order order);
    void Cancel(Order order);
    string Name { get; }
}

public class PlacedState : IOrderState
{
    public string Name => "Placed";
    public void Pay(Order order) => order.SetState(new PaidState());
    public void Ship(Order order) => throw new InvalidOperationException("Cannot ship an unpaid order.");
    public void Cancel(Order order) => order.SetState(new CancelledState());
}

public class PaidState : IOrderState
{
    public string Name => "Paid";
    public void Pay(Order order) => throw new InvalidOperationException("Order is already paid.");
    public void Ship(Order order) => order.SetState(new ShippedState());
    public void Cancel(Order order) => order.SetState(new CancelledState());
}

public class ShippedState : IOrderState
{
    public string Name => "Shipped";
    public void Pay(Order order) => throw new InvalidOperationException("Order is already paid.");
    public void Ship(Order order) => throw new InvalidOperationException("Order is already shipped.");
    public void Cancel(Order order) => throw new InvalidOperationException("Cannot cancel a shipped order.");
}

public class CancelledState : IOrderState
{
    public string Name => "Cancelled";
    public void Pay(Order order) => throw new InvalidOperationException("Order is cancelled.");
    public void Ship(Order order) => throw new InvalidOperationException("Order is cancelled.");
    public void Cancel(Order order) => throw new InvalidOperationException("Order is already cancelled.");
}

// Order delegates every lifecycle action to its current state and never
// contains a status-checking if/switch itself.
public class Order
{
    private IOrderState _state = new PlacedState();

    public string Status => _state.Name;

    public void SetState(IOrderState state) => _state = state;

    public void Pay() => _state.Pay(this);
    public void Ship() => _state.Ship(this);
    public void Cancel() => _state.Cancel(this);
}

public static class StateDemo
{
    public static void Run()
    {
        var order = new Order();
        Console.WriteLine(order.Status); // Placed

        order.Pay();
        Console.WriteLine(order.Status); // Paid

        order.Ship();
        Console.WriteLine(order.Status); // Shipped

        try
        {
            order.Cancel(); // illegal from Shipped — throws, by construction
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Rejected: {ex.Message}");
        }
    }
}
