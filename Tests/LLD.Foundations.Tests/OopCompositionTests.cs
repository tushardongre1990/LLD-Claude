using Foundations.OopBasics.Composition;

namespace LLD.Foundations.Tests;

// Covers 00-Foundations/01-OOP-Basics §4 — composition vs inheritance.
// Kept in its own file because Foundations.OopBasics and
// Foundations.OopBasics.Composition each define a `Car`.

public class CompositionTests
{
    // Test doubles: substituting a part is trivial precisely because Car
    // depends on the IEngine / ITransmission abstractions, not on concretes.
    // That testability is itself an argument for composition.
    private sealed class RecordingEngine : IEngine
    {
        public int IgniteCount { get; private set; }
        public void Ignite() => IgniteCount++;
    }

    private sealed class RecordingTransmission : ITransmission
    {
        public int EngageCount { get; private set; }
        public void Engage() => EngageCount++;
    }

    [Fact]
    public void Start_DelegatesToTheInjectedParts()
    {
        var engine = new RecordingEngine();
        var transmission = new RecordingTransmission();

        new Car("Test Mule", engine, transmission).Start();

        Assert.Equal(1, engine.IgniteCount);
        Assert.Equal(1, transmission.EngageCount);
    }

    [Fact]
    public void OneCarType_CoversEveryCombinationOfParts()
    {
        // The payoff over inheritance: 2 engines x 2 transmissions is 4
        // behaviours from 4 small classes, not 4 subclasses of Car.
        IEngine[] engines = [new PetrolEngine(), new ElectricEngine()];
        ITransmission[] transmissions = [new ManualTransmission(), new AutomaticTransmission()];

        var cars = (from e in engines
                    from t in transmissions
                    select new Car("Combo", e, t)).ToList();

        Assert.Equal(4, cars.Count);
        foreach (var car in cars)
            car.Start(); // every combination is a valid, working Car
    }
}
