// Illustrates notes.md in this folder — the Command pattern.
//   dotnet run --project Runner command

namespace Foundations.Patterns.Behavioral.Command;

public interface ICommand
{
    void Execute();
    void Undo();
}

// The receiver: knows how to actually perform the action. Has no idea
// Command objects exist.
public class Light
{
    public bool IsOn { get; private set; }
    public void On() { IsOn = true; Console.WriteLine("Light ON"); }
    public void Off() { IsOn = false; Console.WriteLine("Light OFF"); }
}

public class TurnOnCommand : ICommand
{
    private readonly Light _light;
    public TurnOnCommand(Light light) => _light = light;
    public void Execute() => _light.On();
    public void Undo() => _light.Off();
}

public class TurnOffCommand : ICommand
{
    private readonly Light _light;
    public TurnOffCommand(Light light) => _light = light;
    public void Execute() => _light.Off();
    public void Undo() => _light.On();
}

// The invoker: knows only ICommand, never Light.
//
// Two stacks give you undo AND redo:
//   - executing a command pushes it onto _undo and CLEARS _redo
//     (once you take a new action, the old redo branch is unreachable —
//      this is how every real editor behaves)
//   - undo pops from _undo, reverses it, pushes onto _redo
//   - redo pops from _redo, re-executes it, pushes back onto _undo
public class RemoteControl
{
    private readonly Stack<ICommand> _undo = new();
    private readonly Stack<ICommand> _redo = new();

    public void PressButton(ICommand command)
    {
        command.Execute();
        _undo.Push(command);
        _redo.Clear(); // a new action invalidates the redo branch
    }

    public bool PressUndo()
    {
        if (_undo.Count == 0) return false;

        var command = _undo.Pop();
        command.Undo();
        _redo.Push(command);
        return true;
    }

    public bool PressRedo()
    {
        if (_redo.Count == 0) return false;

        var command = _redo.Pop();
        command.Execute();
        _undo.Push(command);
        return true;
    }

    public int UndoDepth => _undo.Count;
    public int RedoDepth => _redo.Count;
}

public static class CommandDemo
{
    public static void Run()
    {
        var light = new Light();
        var remote = new RemoteControl();

        remote.PressButton(new TurnOnCommand(light));  // Light ON
        remote.PressButton(new TurnOffCommand(light)); // Light OFF

        remote.PressUndo();                            // Light ON  (undoes TurnOff)
        remote.PressUndo();                            // Light OFF (undoes TurnOn)

        remote.PressRedo();                            // Light ON  (replays TurnOn)
        Console.WriteLine($"Light is on: {light.IsOn}, redo depth: {remote.RedoDepth}");
    }
}
