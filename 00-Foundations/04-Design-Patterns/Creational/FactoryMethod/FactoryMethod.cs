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

// ---------------------------------------------------------------------
// 1. SIMPLE FACTORY (a.k.a. "Static Factory") — NOT a GoF pattern.
//
// One place owns the type->class mapping. This is what most people
// (and most LLD interview answers) actually mean when they say
// "I'll use a factory," and for most case studies it is the right,
// proportionate choice.
//
// Trade-off worth naming out loud: adding a new VehicleType still means
// editing this switch. It centralizes the decision to ONE place instead
// of scattering `new Car()` across the codebase — a big win — but it is
// not itself open/closed.
// ---------------------------------------------------------------------
public static class SimpleVehicleFactory
{
    public static Vehicle Create(VehicleType type) => type switch
    {
        VehicleType.Car => new Car(),
        VehicleType.Motorcycle => new Motorcycle(),
        VehicleType.Truck => new Truck(),
        _ => throw new ArgumentOutOfRangeException(nameof(type)),
    };
}

// ---------------------------------------------------------------------
// 2. FACTORY METHOD (the actual GoF pattern).
//
// The defining trait: a *creator class hierarchy* where an abstract
// method defers the choice of concrete product to SUBCLASSES. There is
// no switch anywhere — picking a subclass IS picking the product.
//
// The creator usually also contains shared logic that operates on
// whatever the factory method returns (SubmitToLot() below). That shared
// logic is the reason the pattern exists as a hierarchy rather than a
// single static method — otherwise Simple Factory would do.
// ---------------------------------------------------------------------
public abstract class VehicleRegistration
{
    // The factory method — subclasses decide the concrete Vehicle.
    protected abstract Vehicle CreateVehicle();

    // Shared workflow that every registration performs, written once,
    // against the abstract Vehicle type.
    public decimal SubmitToLot(int hours)
    {
        Vehicle vehicle = CreateVehicle();
        decimal fee = vehicle.CalculateFee(hours);
        Console.WriteLine($"Registered {vehicle.GetType().Name}, fee {fee:C}");
        return fee;
    }
}

public class CarRegistration : VehicleRegistration
{
    protected override Vehicle CreateVehicle() => new Car();
}

public class MotorcycleRegistration : VehicleRegistration
{
    protected override Vehicle CreateVehicle() => new Motorcycle();
}

// Adding a Truck flow = one new subclass. No existing class is edited,
// and there is no switch to extend. THIS is the open/closed property
// that Simple Factory lacks.
public class TruckRegistration : VehicleRegistration
{
    protected override Vehicle CreateVehicle() => new Truck();
}

public static class FactoryMethodDemo
{
    public static void Run()
    {
        // Simple Factory: caller supplies a type token.
        Vehicle v = SimpleVehicleFactory.Create(VehicleType.Truck);
        Console.WriteLine(v.CalculateFee(3));

        // Factory Method: caller picks a creator subclass; the product
        // type follows from it.
        VehicleRegistration registration = new MotorcycleRegistration();
        registration.SubmitToLot(hours: 3);
    }
}
