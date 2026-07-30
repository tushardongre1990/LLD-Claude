namespace Foundations.Patterns.Behavioral.Memento;

// The memento: immutable snapshot. Its constructor is internal-ish by
// convention — only TextEditor is meant to create meaningful ones.
public sealed class EditorMemento
{
    internal string Content { get; }
    internal EditorMemento(string content) => Content = content;
}

// The originator: owns real state, controls exactly what a snapshot means
// and how to restore from one.
public class TextEditor
{
    private string _content = "";

    public void Type(string text) => _content += text;

    public string Content => _content;

    public EditorMemento Save() => new(_content);

    public void Restore(EditorMemento memento) => _content = memento.Content;
}

// The caretaker: stores mementos for undo, never inspects their contents.
public class History
{
    private readonly Stack<EditorMemento> _mementos = new();

    public void Push(EditorMemento memento) => _mementos.Push(memento);

    public EditorMemento? Pop() => _mementos.Count > 0 ? _mementos.Pop() : null;
}

public static class MementoDemo
{
    public static void Run()
    {
        var editor = new TextEditor();
        var history = new History();

        editor.Type("Hello");
        history.Push(editor.Save()); // checkpoint after "Hello"

        editor.Type(", world!");
        history.Push(editor.Save()); // checkpoint after "Hello, world!"

        editor.Type(" Oops typo");
        Console.WriteLine(editor.Content); // "Hello, world! Oops typo"

        var lastGood = history.Pop();
        if (lastGood != null) editor.Restore(lastGood);
        Console.WriteLine(editor.Content); // "Hello, world!"
    }
}
