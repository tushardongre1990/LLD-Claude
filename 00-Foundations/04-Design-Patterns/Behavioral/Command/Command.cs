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

// The invoker: knows only ICommand, never Light. Maintains history so any
// executed command can be undone in reverse order.
public class RemoteControl
{
    private readonly Stack<ICommand> _history = new();

    public void PressButton(ICommand command)
    {
        command.Execute();
        _history.Push(command);
    }

    public void PressUndo()
    {
        if (_history.Count == 0) return;
        _history.Pop().Undo();
    }
}

public static class CommandDemo
{
    public static void Run()
    {
        var light = new Light();
        var remote = new RemoteControl();

        remote.PressButton(new TurnOnCommand(light));  // Light ON
        remote.PressButton(new TurnOffCommand(light)); // Light OFF
        remote.PressUndo();                             // Light ON  (undoes TurnOff)
        remote.PressUndo();                             // Light OFF (undoes TurnOn)
    }
}
