// Illustrates notes.md §4 — Composition vs Inheritance ("has-a" vs "is-a").
//   dotnet run --project Runner composition

namespace Foundations.OopBasics.Composition;

// Composition: a Car HAS-A engine, rather than IS-A petrol car / IS-A electric
// car. The behaviour that varies lives in its own object and gets injected, so
// Car itself never needs subclassing to gain a new variant.

public interface IEngine
{
    void Ignite();
}

public class PetrolEngine : IEngine
{
    public void Ignite() => Console.WriteLine("  starter motor cranks, fuel injects, idles at 800rpm");
}

public class ElectricEngine : IEngine
{
    public void Ignite() => Console.WriteLine("  silent, instant torque");
}

// A second axis that varies independently of the engine.
public interface ITransmission
{
    void Engage();
}

public class ManualTransmission : ITransmission
{
    public void Engage() => Console.WriteLine("  clutch in, first gear");
}

public class AutomaticTransmission : ITransmission
{
    public void Engage() => Console.WriteLine("  shift to D");
}

// One concrete Car. It delegates the varying parts instead of owning them.
public class Car
{
    private readonly string _model;
    private readonly IEngine _engine;
    private readonly ITransmission _transmission;

    public Car(string model, IEngine engine, ITransmission transmission)
    {
        _model = model;
        _engine = engine;
        _transmission = transmission;
    }

    public void Start()
    {
        Console.WriteLine($"{_model}:");
        _engine.Ignite();
        _transmission.Engage();
    }
}

public static class CompositionDemo
{
    public static void Run()
    {
        // Same Car class every time — only the injected parts differ.
        new Car("Hatchback", new PetrolEngine(), new ManualTransmission()).Start();
        new Car("City EV", new ElectricEngine(), new AutomaticTransmission()).Start();
        new Car("Sport EV", new ElectricEngine(), new ManualTransmission()).Start();

        // Why this beats a hierarchy here: two independent axes of variation.
        // With inheritance you'd need PetrolManualCar, PetrolAutomaticCar,
        // ElectricManualCar, ElectricAutomaticCar — 2 x 2 subclasses, and 3 x 3
        // the moment a third engine and a third transmission appear. With
        // composition the axes stay separate and combine at runtime.

        // This is not a verdict against inheritance. Vehicle -> Car/Motorcycle
        // in Inheritance.cs is a genuine taxonomy with shared behaviour and one
        // axis of variation, and a hierarchy models it well. Composition earns
        // its keep when the variation is in a *part*, or when the axes multiply.
    }
}
