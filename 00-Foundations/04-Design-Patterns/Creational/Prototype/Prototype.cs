namespace Foundations.Patterns.Creational.Prototype;

public class Item
{
    public string Name { get; set; } = "";
    public int Quantity { get; set; }

    public Item Clone() => new() { Name = Name, Quantity = Quantity };
}

public interface IPrototype<T>
{
    T Clone();
}

public class Order : IPrototype<Order>
{
    public string CustomerId { get; set; } = "";
    public List<Item> Items { get; set; } = new();

    // Shallow copy: reuses the SAME Item objects. Mutating an item on the
    // clone would also mutate the original's item — usually a bug.
    public Order ShallowClone() => (Order)MemberwiseClone();

    // Deep copy: clone every nested mutable reference too, so the copy is
    // fully independent of the original.
    public Order Clone()
    {
        var copy = (Order)MemberwiseClone();
        copy.Items = Items.Select(i => i.Clone()).ToList();
        return copy;
    }
}

public static class PrototypeDemo
{
    public static void Run()
    {
        var original = new Order
        {
            CustomerId = "C-1",
            Items = new List<Item> { new() { Name = "Pizza", Quantity = 1 } },
        };

        var shallow = original.ShallowClone();
        shallow.Items[0].Quantity = 99;
        Console.WriteLine(original.Items[0].Quantity); // 99 — bug! original mutated too

        var original2 = new Order
        {
            CustomerId = "C-2",
            Items = new List<Item> { new() { Name = "Pizza", Quantity = 1 } },
        };
        var deep = original2.Clone();
        deep.Items[0].Quantity = 99;
        Console.WriteLine(original2.Items[0].Quantity); // 1 — untouched, correctly independent
    }
}
