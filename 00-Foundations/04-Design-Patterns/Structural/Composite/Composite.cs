// Illustrates notes.md in this folder — the Composite pattern.
//   dotnet run --project Runner composite

namespace Foundations.Patterns.Structural.Composite;

public abstract class FileSystemEntry
{
    public string Name { get; }
    protected FileSystemEntry(string name) => Name = name;

    public abstract long GetSize();
}

// Leaf: no children, returns its own size.
public class File : FileSystemEntry
{
    private readonly long _sizeBytes;

    public File(string name, long sizeBytes) : base(name)
    {
        _sizeBytes = sizeBytes;
    }

    public override long GetSize() => _sizeBytes;
}

// Composite: contains other entries (leaves or subtrees) and delegates,
// recursively summing. Callers never need to distinguish File vs Folder.
public class Folder : FileSystemEntry
{
    private readonly List<FileSystemEntry> _children = new();

    public Folder(string name) : base(name) { }

    public void Add(FileSystemEntry entry) => _children.Add(entry);

    public override long GetSize() => _children.Sum(c => c.GetSize());
}

public static class CompositeDemo
{
    public static void Run()
    {
        var root = new Folder("root");
        var docs = new Folder("docs");
        docs.Add(new File("resume.pdf", 200_000));
        docs.Add(new File("notes.txt", 5_000));

        var photos = new Folder("photos");
        photos.Add(new File("vacation.jpg", 3_000_000));

        root.Add(docs);
        root.Add(photos);
        root.Add(new File("readme.md", 1_200));

        // One call, works uniformly whether root has 1 file or a
        // thousand nested folders.
        Console.WriteLine($"Total size: {root.GetSize()} bytes");
    }
}
