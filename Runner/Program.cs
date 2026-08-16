// Runs any foundation demo by name so you can see a pattern execute.
//
//   dotnet run --project Runner                 → lists everything
//   dotnet run --project Runner strategy        → runs the Strategy demo
//   dotnet run --project Runner all             → runs every demo

var demos = new Dictionary<string, (string Description, Action Run)>(StringComparer.OrdinalIgnoreCase)
{
    // --- OOP Basics ---
    ["encapsulation"] = ("OOP: invariants enforced behind methods", Foundations.OopBasics.EncapsulationDemo.Run),
    ["abstraction"] = ("OOP: depending on an interface, not an implementation", Foundations.OopBasics.AbstractionDemo.Run),
    ["inheritance"] = ("OOP: abstract base class + overrides", Foundations.OopBasics.InheritanceDemo.Run),
    ["polymorphism"] = ("OOP: runtime dispatch over a base-type list", Foundations.OopBasics.PolymorphismDemo.Run),

    // --- SOLID ---
    ["srp"] = ("SOLID: one reason to change per class", Foundations.Solid.Srp.Fixed.SrpDemo.Run),
    ["ocp"] = ("SOLID: extend by adding classes, not editing them", Foundations.Solid.Ocp.Fixed.OcpDemo.Run),
    ["lsp-violation"] = ("SOLID: Square/Rectangle breaking substitutability", Foundations.Solid.Lsp.Violation.LspViolationDemo.Run),
    ["lsp"] = ("SOLID: shapes sharing only what they truly share", Foundations.Solid.Lsp.Fixed.LspFixedDemo.Run),
    ["isp"] = ("SOLID: role interfaces instead of one fat interface", Foundations.Solid.Isp.Fixed.IspDemo.Run),
    ["dip"] = ("SOLID: injecting an abstraction for testability", Foundations.Solid.Dip.Fixed.DipDemo.Run),

    // --- Creational patterns ---
    ["singleton"] = ("Creational: one instance, thread-safe variants", Foundations.Patterns.Creational.Singleton.SingletonDemo.Run),
    ["factory"] = ("Creational: Simple Factory vs GoF Factory Method", Foundations.Patterns.Creational.FactoryMethod.FactoryMethodDemo.Run),
    ["abstractfactory"] = ("Creational: families of related products", Foundations.Patterns.Creational.AbstractFactory.AbstractFactoryDemo.Run),
    ["builder"] = ("Creational: step-by-step construction, immutable result", Foundations.Patterns.Creational.Builder.BuilderDemo.Run),
    ["prototype"] = ("Creational: shallow vs deep clone", Foundations.Patterns.Creational.Prototype.PrototypeDemo.Run),

    // --- Structural patterns ---
    ["adapter"] = ("Structural: making an incompatible SDK fit", Foundations.Patterns.Structural.Adapter.AdapterDemo.Run),
    ["decorator"] = ("Structural: stacking behavior at runtime", Foundations.Patterns.Structural.Decorator.DecoratorDemo.Run),
    ["facade"] = ("Structural: one entry point over many services", Foundations.Patterns.Structural.Facade.FacadeDemo.Run),
    ["composite"] = ("Structural: uniform treatment of leaves and trees", Foundations.Patterns.Structural.Composite.CompositeDemo.Run),
    ["proxy"] = ("Structural: lazy loading behind the same interface", Foundations.Patterns.Structural.Proxy.ProxyDemo.Run),
    ["flyweight"] = ("Structural: sharing intrinsic state across many objects", Foundations.Patterns.Structural.Flyweight.FlyweightDemo.Run),
    ["bridge"] = ("Structural: two hierarchies varying independently", Foundations.Patterns.Structural.Bridge.BridgeDemo.Run),

    // --- Behavioral patterns ---
    ["strategy"] = ("Behavioral: interchangeable algorithms", Foundations.Patterns.Behavioral.Strategy.StrategyDemo.Run),
    ["observer"] = ("Behavioral: one-to-many notification", Foundations.Patterns.Behavioral.Observer.ObserverDemo.Run),
    ["state"] = ("Behavioral: lifecycle with illegal transitions rejected", Foundations.Patterns.Behavioral.State.StateDemo.Run),
    ["command"] = ("Behavioral: actions as objects, with undo", Foundations.Patterns.Behavioral.Command.CommandDemo.Run),
    ["chain"] = ("Behavioral: request passed along handlers", Foundations.Patterns.Behavioral.ChainOfResponsibility.ChainOfResponsibilityDemo.Run),
    ["template"] = ("Behavioral: fixed skeleton, varying steps", Foundations.Patterns.Behavioral.TemplateMethod.TemplateMethodDemo.Run),
    ["iterator"] = ("Behavioral: traversal by hand vs IEnumerable<T>", Foundations.Patterns.Behavioral.Iterator.IteratorDemo.Run),
    ["mediator"] = ("Behavioral: peers communicating through a hub", Foundations.Patterns.Behavioral.Mediator.MediatorDemo.Run),
    ["memento"] = ("Behavioral: snapshot and restore without breaking encapsulation", Foundations.Patterns.Behavioral.Memento.MementoDemo.Run),
    ["visitor"] = ("Behavioral: new operations over a stable hierarchy", Foundations.Patterns.Behavioral.Visitor.VisitorDemo.Run),

    // --- Concurrency ---
    ["concurrency"] = ("Concurrency: race condition, lock, optimistic versioning", Foundations.Concurrency.ConcurrencyDemo.Run),
};

if (args.Length == 0)
{
    Console.WriteLine("Usage: dotnet run --project Runner <demo>\n");
    Console.WriteLine("Available demos:\n");
    foreach (var (name, demo) in demos.OrderBy(d => d.Key))
        Console.WriteLine($"  {name,-18} {demo.Description}");
    Console.WriteLine("\n  all                Run every demo in sequence");
    return;
}

if (string.Equals(args[0], "all", StringComparison.OrdinalIgnoreCase))
{
    foreach (var (name, demo) in demos.OrderBy(d => d.Key))
    {
        Console.WriteLine($"\n===== {name} =====");
        demo.Run();
    }
    return;
}

if (!demos.TryGetValue(args[0], out var selected))
{
    Console.WriteLine($"Unknown demo '{args[0]}'. Run without arguments to list them.");
    return;
}

Console.WriteLine($"===== {args[0]} =====");
selected.Run();
