namespace Foundations.Patterns.Structural.Proxy;

public interface IImage
{
    void Display();
}

// Expensive to construct — imagine this reads a large file from disk.
public class RealImage : IImage
{
    private readonly string _filename;

    public RealImage(string filename)
    {
        _filename = filename;
        LoadFromDisk();
    }

    private void LoadFromDisk() => Console.WriteLine($"Loading {_filename} from disk...");

    public void Display() => Console.WriteLine($"Displaying {_filename}");
}

// Virtual Proxy: same interface as RealImage, but defers the expensive
// construction until Display() is actually called.
public class ProxyImage : IImage
{
    private readonly string _filename;
    private RealImage? _real;

    public ProxyImage(string filename)
    {
        _filename = filename; // cheap — no disk load yet
    }

    public void Display()
    {
        _real ??= new RealImage(_filename); // load lazily, only once
        _real.Display();
    }
}

public static class ProxyDemo
{
    public static void Run()
    {
        IImage image = new ProxyImage("vacation.jpg"); // instant, no disk I/O yet
        Console.WriteLine("Proxy created, image not loaded yet.");

        image.Display(); // triggers the real load, exactly once
        image.Display(); // reuses the already-loaded RealImage
    }
}
