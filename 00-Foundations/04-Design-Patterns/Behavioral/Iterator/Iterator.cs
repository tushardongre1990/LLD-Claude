// Illustrates notes.md in this folder — the Iterator pattern.
//   dotnet run --project Runner iterator

using System.Collections;

namespace Foundations.Patterns.Behavioral.Iterator;

// --- 1. The pattern implemented by hand, exactly as GoF describes it ---
public interface IIterator<T>
{
    bool HasNext();
    T Next();
}

public class ListIterator<T> : IIterator<T>
{
    private readonly List<T> _items;
    private int _position;

    public ListIterator(List<T> items) => _items = items;

    public bool HasNext() => _position < _items.Count;
    public T Next() => _items[_position++];
}

public class BrowserHistory
{
    private readonly List<string> _urls = new();

    public void Visit(string url) => _urls.Add(url);

    // Client gets an iterator, never the raw List<string>.
    public IIterator<string> CreateIterator() => new ListIterator<string>(_urls);
}

// --- 2. The idiomatic C# way: implement IEnumerable<T> and get `foreach`,
//    LINQ, spread-like usage for free. `yield return` builds the iterator
//    (a state machine) for you — this IS the Iterator pattern, just with
//    language support instead of a hand-rolled IIterator interface. ---
public class BrowserHistoryEnumerable : IEnumerable<string>
{
    private readonly List<string> _urls = new();

    public void Visit(string url) => _urls.Add(url);

    public IEnumerator<string> GetEnumerator()
    {
        foreach (var url in _urls)
            yield return url;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
}

public static class IteratorDemo
{
    public static void Run()
    {
        var history = new BrowserHistory();
        history.Visit("a.com");
        history.Visit("b.com");

        var it = history.CreateIterator();
        while (it.HasNext())
            Console.WriteLine(it.Next());

        var history2 = new BrowserHistoryEnumerable();
        history2.Visit("c.com");
        history2.Visit("d.com");

        foreach (var url in history2) // works because of IEnumerable<T>
            Console.WriteLine(url);
    }
}
