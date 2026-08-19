# Decorator

**Category**: Structural
**Intent**: Attach additional responsibilities to an object **dynamically**,
as an alternative to subclassing for every combination of behavior.

## The problem: subclass explosion

Modeling a coffee shop's add-ons with inheritance requires a class per
*combination*: `CoffeeWithMilk`, `CoffeeWithMilkAndSugar`,
`CoffeeWithMilkAndSugarAndWhip`... — combinatorial blowup, and it's fixed at
compile time (can't add "extra shot" to an already-ordered coffee at
runtime).

## Structure

```mermaid
classDiagram
    class Beverage {
        <<abstract>>
        +Cost() decimal
        +Description() string
    }
    class Espresso
    class BeverageDecorator {
        <<abstract>>
        #inner : Beverage
        +Cost() decimal
        +Description() string
    }
    class MilkDecorator
    class SugarDecorator

    Beverage <|-- Espresso
    Beverage <|-- BeverageDecorator
    BeverageDecorator <|-- MilkDecorator
    BeverageDecorator <|-- SugarDecorator
    BeverageDecorator o-- Beverage : wraps
```

```csharp
// The decorator IS-A Beverage and HAS-A Beverage. That double relationship
// is the whole trick — it's what lets decorators wrap each other.
public abstract class BeverageDecorator : Beverage
{
    protected readonly Beverage Inner;
    protected BeverageDecorator(Beverage inner) => Inner = inner;
}

// Each concrete decorator adds only its own delta, then delegates.
public class MilkDecorator : BeverageDecorator
{
    public override decimal Cost()       => Inner.Cost() + 0.50m;
    public override string Description() => Inner.Description() + " + Milk";
}

public class SugarDecorator : BeverageDecorator { /* + 0.25m */ }
public class WhipDecorator  : BeverageDecorator { /* + 0.75m */ }

// Any combination, stacked at runtime, no new class needed:
Beverage order = new WhipDecorator(new SugarDecorator(new MilkDecorator(new Espresso())));
Console.WriteLine($"{order.Description()} = {order.Cost():C}");
// Espresso + Milk + Sugar + Whip = $3.50
```

Each decorator **wraps** a `Beverage` (which might itself be another
decorator) and **is-a** `Beverage` itself — so decorators stack. Every layer
adds its own cost/description on top of delegating to the wrapped instance.

Trace `Cost()` through that chain once, outside-in: `Whip` asks `Sugar`,
which asks `Milk`, which asks `Espresso` (2.00), and the additions unwind
back out — 2.00 → 2.50 → 2.75 → 3.50. Being able to narrate that recursion
is what tells an interviewer you understand the pattern rather than
recognizing its diagram.

📄 [`Decorator.cs`](Decorator.cs) · `dotnet run --project Runner decorator`

> **Try it:** stack the same decorator twice — `new MilkDecorator(new
> MilkDecorator(new Espresso()))`. It works, and charges for both. Whether
> that's a feature (double shot) or a bug (duplicate add-on) is a domain
> question the pattern won't answer for you, and "how do you prevent invalid
> combinations?" is the natural follow-up.

## When to use

- You need to add responsibilities to **individual objects**, not to every
  instance of a class, and combinations should be composable at runtime
  (any subset of add-ons, in any order).
- Classic real examples: coffee shop add-ons, gift-wrapping an order,
  Java/C# I/O streams (`BufferedStream` wrapping a `FileStream`), adding
  scrollbars/borders to a UI component.

## Decorator vs Inheritance

Inheritance picks behavior **once, at compile time, per class**. Decorator
composes behavior **at runtime, per instance**, and lets you stack any
combination without a new class per combination — directly the "favor
composition over inheritance" principle from `01-OOP-Basics`.

## Decorator vs Proxy — another common mix-up

Both wrap an object behind the same interface. **Decorator adds behavior**
(new responsibilities layered on). **Proxy controls access** (permission
checks, lazy loading, caching) without adding new responsibilities — see
`../Proxy/notes.md`.

## Interview variations

- "Customer wants coffee with milk, extra shot, and whipped cream, in any
  combination — how do you model the pricing without a class per
  combination?" → Decorator, with the stacking diagram.
- "How is this different from just adding optional constructor
  parameters?" → decorators can be added/removed/reordered independently
  and composed at runtime; a parameter list is fixed at construction and
  doesn't scale to many independent optional behaviors.
