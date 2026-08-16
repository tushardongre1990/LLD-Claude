// Illustrates notes.md in this folder — the State pattern.
//   dotnet run --project Runner state

namespace Foundations.Patterns.Behavioral.State;

public interface IOrderState
{
    string Name { get; }
    void Pay(Order order);
    void Ship(Order order);
    void Deliver(Order order);
    void Cancel(Order order);
}

// Each state knows exactly which transitions are legal FROM ITSELF.
// Everything not implemented is rejected — illegal transitions are
// impossible to reach rather than merely discouraged.
public class PlacedState : IOrderState
{
    public string Name => "Placed";
    public void Pay(Order order) => order.TransitionTo(new PaidState());
    public void Ship(Order order) => throw new InvalidOperationException("Cannot ship an unpaid order.");
    public void Deliver(Order order) => throw new InvalidOperationException("Cannot deliver an unshipped order.");
    public void Cancel(Order order) => order.TransitionTo(new CancelledState());
}

public class PaidState : IOrderState
{
    public string Name => "Paid";
    public void Pay(Order order) => throw new InvalidOperationException("Order is already paid.");
    public void Ship(Order order) => order.TransitionTo(new ShippedState());
    public void Deliver(Order order) => throw new InvalidOperationException("Cannot deliver an unshipped order.");
    public void Cancel(Order order) => order.TransitionTo(new CancelledState());
}

public class ShippedState : IOrderState
{
    public string Name => "Shipped";
    public void Pay(Order order) => throw new InvalidOperationException("Order is already paid.");
    public void Ship(Order order) => throw new InvalidOperationException("Order is already shipped.");
    public void Deliver(Order order) => order.TransitionTo(new DeliveredState());
    public void Cancel(Order order) => throw new InvalidOperationException("Cannot cancel a shipped order.");
}

// Terminal state: no transitions out.
public class DeliveredState : IOrderState
{
    public string Name => "Delivered";
    public void Pay(Order order) => throw new InvalidOperationException("Order is complete.");
    public void Ship(Order order) => throw new InvalidOperationException("Order is complete.");
    public void Deliver(Order order) => throw new InvalidOperationException("Order is already delivered.");
    public void Cancel(Order order) => throw new InvalidOperationException("Cannot cancel a delivered order.");
}

// Terminal state: no transitions out.
public class CancelledState : IOrderState
{
    public string Name => "Cancelled";
    public void Pay(Order order) => throw new InvalidOperationException("Order is cancelled.");
    public void Ship(Order order) => throw new InvalidOperationException("Order is cancelled.");
    public void Deliver(Order order) => throw new InvalidOperationException("Order is cancelled.");
    public void Cancel(Order order) => throw new InvalidOperationException("Order is already cancelled.");
}

public class Order
{
    private IOrderState _state = new PlacedState();

    public string Status => _state.Name;

    // `internal`, not `public`: the state classes (same assembly) drive
    // transitions, but outside callers CANNOT slam the order into an
    // arbitrary state and bypass the rules. A public SetState would
    // defeat the entire point of the pattern — the invariant "only legal
    // transitions happen" would no longer be enforced by the design.
    internal void TransitionTo(IOrderState next) => _state = next;

    // The Order itself contains no status-checking if/switch at all —
    // it just delegates to whatever state it is currently in.
    public void Pay() => _state.Pay(this);
    public void Ship() => _state.Ship(this);
    public void Deliver() => _state.Deliver(this);
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

        order.Deliver();
        Console.WriteLine(order.Status); // Delivered

        try
        {
            order.Cancel(); // illegal from Delivered — rejected by construction
        }
        catch (InvalidOperationException ex)
        {
            Console.WriteLine($"Rejected: {ex.Message}");
        }
    }
}
