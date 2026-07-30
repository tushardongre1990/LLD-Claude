namespace Foundations.Solid.Ocp;

namespace Violation
{
    public enum VehicleType { Car, Motorcycle }

    // Every new vehicle type means editing this method — closed for
    // extension, wide open for regressions in existing branches.
    public class FeeCalculator
    {
        public decimal Calculate(VehicleType type, int hours) => type switch
        {
            VehicleType.Car => 20m + 10m * hours,
            VehicleType.Motorcycle => 10m + 5m * hours,
            _ => throw new ArgumentOutOfRangeException(nameof(type)),
        };
    }
}

namespace Fixed
{
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

    // Adding Truck later = a new class. Zero edits here or in Car/Motorcycle.
    public class Truck : Vehicle
    {
        public override decimal CalculateFee(int hours) => 40m + 20m * hours;
    }

    public static class OcpDemo
    {
        public static void Run()
        {
            var vehicles = new List<Vehicle> { new Car(), new Motorcycle(), new Truck() };
            foreach (var v in vehicles)
                Console.WriteLine(v.CalculateFee(2));
        }
    }
}
