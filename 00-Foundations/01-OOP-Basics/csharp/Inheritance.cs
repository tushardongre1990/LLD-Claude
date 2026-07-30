namespace Foundations.OopBasics;

// Inheritance: genuine "is-a" taxonomy. Vehicle holds shared state + shared
// behavior; subclasses provide the parts that differ.
public abstract class Vehicle
{
    public string LicensePlate { get; }

    protected Vehicle(string licensePlate)
    {
        LicensePlate = licensePlate;
    }

    // Shared behavior, common to every vehicle.
    public void DisplayPlate() => Console.WriteLine($"Plate: {LicensePlate}");

    // Each subtype must supply its own pricing rule.
    public abstract decimal CalculateParkingFee(int hours);
}

public class Car : Vehicle
{
    public Car(string licensePlate) : base(licensePlate) { }

    public override decimal CalculateParkingFee(int hours) => 20m + 10m * hours;
}

public class Motorcycle : Vehicle
{
    public Motorcycle(string licensePlate) : base(licensePlate) { }

    public override decimal CalculateParkingFee(int hours) => 10m + 5m * hours;
}

public static class InheritanceDemo
{
    public static void Run()
    {
        Vehicle car = new Car("KA-01-1234");
        Vehicle bike = new Motorcycle("KA-01-5678");

        car.DisplayPlate();
        Console.WriteLine(car.CalculateParkingFee(3));

        bike.DisplayPlate();
        Console.WriteLine(bike.CalculateParkingFee(3));
    }
}
