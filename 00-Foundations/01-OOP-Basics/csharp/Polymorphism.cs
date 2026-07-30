namespace Foundations.OopBasics;

// Polymorphism: iterate a list of the base type; the correct override runs
// for each concrete type at runtime. No type-checking / switch needed.
public static class PolymorphismDemo
{
    public static void Run()
    {
        var vehicles = new List<Vehicle>
        {
            new Car("KA-01-1111"),
            new Motorcycle("KA-01-2222"),
            new Car("KA-01-3333"),
        };

        foreach (var vehicle in vehicles)
        {
            // Runtime (dynamic) polymorphism: the JIT dispatches to
            // Car.CalculateParkingFee or Motorcycle.CalculateParkingFee
            // based on the actual object type, not the declared type.
            decimal fee = vehicle.CalculateParkingFee(hours: 2);
            Console.WriteLine($"{vehicle.LicensePlate}: {fee:C}");
        }

        // Compile-time polymorphism (overloading) for contrast:
        Console.WriteLine(Add(1, 2));       // int overload
        Console.WriteLine(Add(1.5, 2.5));   // double overload
    }

    private static int Add(int a, int b) => a + b;
    private static double Add(double a, double b) => a + b;
}
