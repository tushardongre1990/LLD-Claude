namespace Foundations.Patterns.Creational.FactoryMethod;

public enum VehicleType { Car, Motorcycle, Truck }

public abstract class Vehicle
{
    public abstract decimal CalculateFee(int hours);
}

public class Car : Vehicle
{
    public override decimal CalculateFee(int hours) => 20m + 10m * hours;
}

public class Motorcycle : Vehicle
{
    public override decimal CalculateFee(int hours) => 10m + 5m * hours;
}

public class Truck : Vehicle
{
    public override decimal CalculateFee(int hours) => 40m + 20m * hours;
}

// The one place that knows how to map a VehicleType to a concrete class.
// Every caller depends only on Vehicle + VehicleType, never on `new Car()`.
public static class VehicleFactory
{
    public static Vehicle Create(VehicleType type) => type switch
    {
        VehicleType.Car => new Car(),
        VehicleType.Motorcycle => new Motorcycle(),
        VehicleType.Truck => new Truck(),
        _ => throw new ArgumentOutOfRangeException(nameof(type)),
    };
}

public static class FactoryMethodDemo
{
    public static void Run()
    {
        Vehicle v = VehicleFactory.Create(VehicleType.Truck);
        Console.WriteLine(v.CalculateFee(3));
    }
}
