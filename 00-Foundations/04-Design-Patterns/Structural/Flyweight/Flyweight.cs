// Illustrates notes.md in this folder — the Flyweight pattern.
//   dotnet run --project Runner flyweight

namespace Foundations.Patterns.Structural.Flyweight;

// Intrinsic state: shared, immutable, expensive-ish (imagine it also holds
// a loaded texture). Reused across every tree of the same species.
public class TreeType
{
    public string Name { get; }
    public string TextureId { get; }

    public TreeType(string name, string textureId)
    {
        Name = name;
        TextureId = textureId;
    }

    public void Draw(int x, int y) =>
        Console.WriteLine($"Drawing {Name} (texture {TextureId}) at ({x},{y})");
}

// The factory ensures each distinct (name, textureId) combination is
// created once and shared, not once per Tree instance.
public class TreeTypeFactory
{
    // Tuple key rather than a concatenated string: no separator-collision
    // bugs (e.g. "a:b" + "c" vs "a" + "b:c"), no allocation to build the
    // key, and the compiler enforces the key's shape.
    private readonly Dictionary<(string Name, string TextureId), TreeType> _pool = new();

    public TreeType GetTreeType(string name, string textureId)
    {
        var key = (name, textureId);
        if (!_pool.TryGetValue(key, out var type))
        {
            type = new TreeType(name, textureId);
            _pool[key] = type;
            Console.WriteLine($"Created new TreeType for {name}/{textureId} (pool size now {_pool.Count})");
        }
        return type;
    }
}

// Extrinsic state: unique per tree — just a position plus a reference to
// the shared flyweight.
public class Tree
{
    private readonly int _x;
    private readonly int _y;
    private readonly TreeType _type;

    public Tree(int x, int y, TreeType type)
    {
        _x = x;
        _y = y;
        _type = type;
    }

    public void Draw() => _type.Draw(_x, _y);
}

public static class FlyweightDemo
{
    public static void Run()
    {
        var factory = new TreeTypeFactory();
        var forest = new List<Tree>();

        var random = new Random(42);
        for (int i = 0; i < 100_000; i++)
        {
            string species = i % 2 == 0 ? "Oak" : "Pine";
            var type = factory.GetTreeType(species, $"{species}Texture"); // only 2 TreeType instances ever created
            forest.Add(new Tree(random.Next(1000), random.Next(1000), type));
        }

        Console.WriteLine($"Trees placed: {forest.Count}, but only 2 shared TreeType objects exist.");
    }
}
