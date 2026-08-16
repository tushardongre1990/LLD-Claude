// Illustrates notes.md in this folder — the Bridge pattern.
//   dotnet run --project Runner bridge

namespace Foundations.Patterns.Structural.Bridge;

// Implementation hierarchy.
public interface IDevice
{
    bool IsOn { get; }
    void TurnOn();
    void TurnOff();
}

public class Tv : IDevice
{
    public bool IsOn { get; private set; }
    public void TurnOn() { IsOn = true; Console.WriteLine("TV on"); }
    public void TurnOff() { IsOn = false; Console.WriteLine("TV off"); }
}

public class Radio : IDevice
{
    public bool IsOn { get; private set; }
    public void TurnOn() { IsOn = true; Console.WriteLine("Radio on"); }
    public void TurnOff() { IsOn = false; Console.WriteLine("Radio off"); }
}

// Abstraction hierarchy — holds a Device via composition (the "bridge"),
// instead of inheriting from it.
public abstract class RemoteControl
{
    protected readonly IDevice Device;

    protected RemoteControl(IDevice device)
    {
        Device = device;
    }

    public void TogglePower()
    {
        if (Device.IsOn) Device.TurnOff();
        else Device.TurnOn();
    }
}

public class BasicRemote : RemoteControl
{
    public BasicRemote(IDevice device) : base(device) { }
}

public class AdvancedRemote : RemoteControl
{
    public AdvancedRemote(IDevice device) : base(device) { }

    public void Mute() => Console.WriteLine("Muted (advanced-only feature)");
}

public static class BridgeDemo
{
    public static void Run()
    {
        // Any remote type x any device type, no combinatorial class needed.
        RemoteControl basicTvRemote = new BasicRemote(new Tv());
        basicTvRemote.TogglePower();

        var advancedRadioRemote = new AdvancedRemote(new Radio());
        advancedRadioRemote.TogglePower();
        advancedRadioRemote.Mute();
    }
}
