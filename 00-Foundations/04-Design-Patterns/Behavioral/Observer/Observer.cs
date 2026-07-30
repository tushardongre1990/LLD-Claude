namespace Foundations.Patterns.Behavioral.Observer;

public interface IStockObserver
{
    void Update(string symbol, decimal price);
}

public interface IStockSubject
{
    void Subscribe(IStockObserver observer);
    void Unsubscribe(IStockObserver observer);
}

// The subject: holds observers only through the shared interface, never
// concrete types.
public class StockTicker : IStockSubject
{
    private readonly List<IStockObserver> _observers = new();
    private readonly string _symbol;
    private decimal _price;

    public StockTicker(string symbol)
    {
        _symbol = symbol;
    }

    public void Subscribe(IStockObserver observer) => _observers.Add(observer);
    public void Unsubscribe(IStockObserver observer) => _observers.Remove(observer);

    public void SetPrice(decimal price)
    {
        _price = price;
        NotifyAll(); // push model: send the new state directly
    }

    private void NotifyAll()
    {
        foreach (var observer in _observers)
            observer.Update(_symbol, _price);
    }
}

public class MobileAppDisplay : IStockObserver
{
    public void Update(string symbol, decimal price) =>
        Console.WriteLine($"[Mobile App] {symbol} is now {price:C}");
}

public class EmailAlert : IStockObserver
{
    private readonly decimal _threshold;

    public EmailAlert(decimal threshold)
    {
        _threshold = threshold;
    }

    public void Update(string symbol, decimal price)
    {
        if (price >= _threshold)
            Console.WriteLine($"[Email] Alert: {symbol} crossed {_threshold:C}, now at {price:C}");
    }
}

public static class ObserverDemo
{
    public static void Run()
    {
        var ticker = new StockTicker("ACME");
        var mobile = new MobileAppDisplay();
        var alert = new EmailAlert(threshold: 100m);

        ticker.Subscribe(mobile);
        ticker.Subscribe(alert);

        ticker.SetPrice(95m);  // only mobile prints
        ticker.SetPrice(105m); // both print

        ticker.Unsubscribe(mobile);
        ticker.SetPrice(110m); // only alert prints
    }
}
