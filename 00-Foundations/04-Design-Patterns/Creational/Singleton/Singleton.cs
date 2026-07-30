namespace Foundations.Patterns.Creational.Singleton;

// 1. Naive lazy Singleton — NOT thread-safe. Two threads can both see
//    _instance == null and both construct one.
public sealed class NaiveParkingLot
{
    private static NaiveParkingLot? _instance;
    private NaiveParkingLot() { }

    public static NaiveParkingLot GetInstance()
    {
        if (_instance == null)
            _instance = new NaiveParkingLot(); // race condition here
        return _instance;
    }
}

// 2. Eager static initialization — thread-safe (CLR guarantees static
//    fields are initialized exactly once, before first use), but the
//    instance is created even if never used.
public sealed class EagerParkingLot
{
    private static readonly EagerParkingLot _instance = new();
    private EagerParkingLot() { }

    public static EagerParkingLot GetInstance() => _instance;
}

// 3. Lazy<T> — thread-safe AND lazy. The idiomatic modern C# approach.
public sealed class LazyParkingLot
{
    private static readonly Lazy<LazyParkingLot> _instance = new(() => new LazyParkingLot());
    private LazyParkingLot() { }

    public static LazyParkingLot GetInstance() => _instance.Value;

    private readonly List<string> _activeTickets = new();
    public void IssueTicket(string ticketId) => _activeTickets.Add(ticketId);
    public int ActiveTicketCount => _activeTickets.Count;
}

// 4. Double-checked locking — manual thread-safe lazy init, for contexts
//    where you can't use Lazy<T> or want tighter control.
public sealed class DoubleCheckedParkingLot
{
    private static DoubleCheckedParkingLot? _instance;
    private static readonly object _lock = new();
    private DoubleCheckedParkingLot() { }

    public static DoubleCheckedParkingLot GetInstance()
    {
        if (_instance == null) // first check, avoids locking on every call
        {
            lock (_lock)
            {
                if (_instance == null) // second check, inside the lock
                    _instance = new DoubleCheckedParkingLot();
            }
        }
        return _instance;
    }
}

public static class SingletonDemo
{
    public static void Run()
    {
        var lot1 = LazyParkingLot.GetInstance();
        var lot2 = LazyParkingLot.GetInstance();
        Console.WriteLine(ReferenceEquals(lot1, lot2)); // true — same instance

        lot1.IssueTicket("T-1");
        Console.WriteLine(lot2.ActiveTicketCount); // 1 — shared state, same object
    }
}
