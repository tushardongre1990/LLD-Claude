using Foundations.Patterns.Behavioral.Command;

namespace LLD.Foundations.Tests;

public class CommandTests
{
    [Fact]
    public void Undo_ReversesTheLastCommand()
    {
        var light = new Light();
        var remote = new RemoteControl();

        remote.PressButton(new TurnOnCommand(light));
        Assert.True(light.IsOn);

        remote.PressUndo();
        Assert.False(light.IsOn);
    }

    [Fact]
    public void Undo_UnwindsInReverseOrder()
    {
        var light = new Light();
        var remote = new RemoteControl();

        remote.PressButton(new TurnOnCommand(light));   // on
        remote.PressButton(new TurnOffCommand(light));  // off

        remote.PressUndo();                             // undoes TurnOff
        Assert.True(light.IsOn);

        remote.PressUndo();                             // undoes TurnOn
        Assert.False(light.IsOn);
    }

    [Fact]
    public void Redo_ReappliesAnUndoneCommand()
    {
        var light = new Light();
        var remote = new RemoteControl();

        remote.PressButton(new TurnOnCommand(light));
        remote.PressUndo();
        Assert.False(light.IsOn);

        Assert.True(remote.PressRedo());
        Assert.True(light.IsOn);
    }

    // Every real editor behaves this way: once you take a new action,
    // the branch you undid is no longer reachable.
    [Fact]
    public void NewAction_ClearsTheRedoBranch()
    {
        var light = new Light();
        var remote = new RemoteControl();

        remote.PressButton(new TurnOnCommand(light));
        remote.PressUndo();
        Assert.Equal(1, remote.RedoDepth);

        remote.PressButton(new TurnOffCommand(light)); // new action
        Assert.Equal(0, remote.RedoDepth);
        Assert.False(remote.PressRedo());
    }

    [Fact]
    public void UndoAndRedo_AreNoOps_WhenStacksAreEmpty()
    {
        var remote = new RemoteControl();

        Assert.False(remote.PressUndo());
        Assert.False(remote.PressRedo());
    }
}
