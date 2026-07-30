namespace Foundations.Patterns.Creational.Builder;

// The finished product is immutable — no setters, only a constructor the
// builder calls once it has gathered everything.
public sealed class Pizza
{
    public string Size { get; }
    public IReadOnlyList<string> Toppings { get; }
    public bool ExtraCheese { get; }

    public Pizza(string size, IReadOnlyList<string> toppings, bool extraCheese)
    {
        Size = size;
        Toppings = toppings;
        ExtraCheese = extraCheese;
    }

    public override string ToString() =>
        $"{Size} pizza with [{string.Join(", ", Toppings)}]{(ExtraCheese ? " + extra cheese" : "")}";
}

public class PizzaBuilder
{
    private string _size = "Medium";
    private readonly List<string> _toppings = new();
    private bool _extraCheese;

    public PizzaBuilder WithSize(string size)
    {
        _size = size;
        return this; // fluent chaining
    }

    public PizzaBuilder AddTopping(string topping)
    {
        _toppings.Add(topping);
        return this;
    }

    public PizzaBuilder WithExtraCheese()
    {
        _extraCheese = true;
        return this;
    }

    public Pizza Build() => new(_size, _toppings, _extraCheese);
}

public static class BuilderDemo
{
    public static void Run()
    {
        Pizza pizza = new PizzaBuilder()
            .WithSize("Large")
            .AddTopping("Mushroom")
            .AddTopping("Olives")
            .WithExtraCheese()
            .Build();

        Console.WriteLine(pizza);
    }
}
