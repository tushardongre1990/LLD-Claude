// Illustrates notes.md in this folder — the Decorator pattern.
//   dotnet run --project Runner decorator

namespace Foundations.Patterns.Structural.Decorator;

public abstract class Beverage
{
    public abstract decimal Cost();
    public abstract string Description();
}

public class Espresso : Beverage
{
    public override decimal Cost() => 2.00m;
    public override string Description() => "Espresso";
}

// The decorator IS-A Beverage and HAS-A Beverage (the wrapped instance) —
// that's the whole trick. Concrete decorators only need to add their own
// delta on top of delegating to `_inner`.
public abstract class BeverageDecorator : Beverage
{
    protected readonly Beverage Inner;

    protected BeverageDecorator(Beverage inner)
    {
        Inner = inner;
    }
}

public class MilkDecorator : BeverageDecorator
{
    public MilkDecorator(Beverage inner) : base(inner) { }
    public override decimal Cost() => Inner.Cost() + 0.50m;
    public override string Description() => Inner.Description() + " + Milk";
}

public class SugarDecorator : BeverageDecorator
{
    public SugarDecorator(Beverage inner) : base(inner) { }
    public override decimal Cost() => Inner.Cost() + 0.25m;
    public override string Description() => Inner.Description() + " + Sugar";
}

public class WhipDecorator : BeverageDecorator
{
    public WhipDecorator(Beverage inner) : base(inner) { }
    public override decimal Cost() => Inner.Cost() + 0.75m;
    public override string Description() => Inner.Description() + " + Whip";
}

public static class DecoratorDemo
{
    public static void Run()
    {
        // Any combination, stacked at runtime, no new class needed:
        Beverage order = new WhipDecorator(new SugarDecorator(new MilkDecorator(new Espresso())));

        Console.WriteLine($"{order.Description()} = {order.Cost():C}");
        // Espresso + Milk + Sugar + Whip = $3.50
    }
}
