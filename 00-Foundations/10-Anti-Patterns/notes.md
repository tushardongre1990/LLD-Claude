# Anti-Patterns

LLD isn't only about producing good designs — it's about **recognizing bad
ones**. A very common interview format is "here's some code/a design, what's
wrong with it?", and the anti-patterns below cover the large majority of
what gets planted in those questions.

Each entry: the smell, why it hurts, and the fix.

---

## God Object / God Service

**Smell**: one class doing everything — `OrderManager` with 40 methods
spanning persistence, pricing, email, and PDF generation. Often named
`...Manager`, `...Helper`, `...Util`, or `...Service`.

**Why it hurts**: every change touches it, nothing can be tested in
isolation, merge conflicts constantly, and no one can hold it in their head.

**Fix**: split along **reasons to change** (SRP). Extract `PricingService`,
`OrderRepository`, `OrderNotifier`. If orchestration remains, that's a
[Facade](../04-Design-Patterns/Structural/Facade/notes.md).

---

## Anemic Domain Model ⭐

**Smell**: classes are pure data bags (properties only, no behavior), and
all the logic lives in service classes that reach into them.

```csharp
// Anemic: Order knows nothing about being an order.
public class Order { public OrderStatus Status { get; set; } }

public class OrderService
{
    public void Cancel(Order o)
    {
        if (o.Status == OrderStatus.Placed || o.Status == OrderStatus.Paid)
            o.Status = OrderStatus.Cancelled;
    }
}
```

**Why it hurts**: the rule "which states can be cancelled" isn't protected
by anything — any other service can set `Status` to whatever it likes. The
invariant exists only by convention.

**Fix**: move behavior onto the entity (`order.Cancel()`), make setters
private, and enforce transitions internally — Tell-Don't-Ask, and
ultimately the [State pattern](../04-Design-Patterns/Behavioral/State/notes.md).

**Interview note**: this is the single most common weakness in candidate
designs. If your class diagram is all nouns with getters and a parallel set
of `XService` classes holding every verb, you have this problem.

---

## Primitive Obsession

**Smell**: important domain concepts represented as bare primitives.

```csharp
void Transfer(decimal amount, string currency, string fromAccount, string toAccount)
```

**Why it hurts**: no validation lives anywhere, arguments are trivially
swappable (`Transfer(100, "INR", to, from)` compiles fine), and rules like
"can't add USD to INR" get duplicated or forgotten.

**Fix**: introduce value objects — `Money`, `AccountId`, `EmailAddress`,
`LicensePlate`. See
[`../07-Domain-Modeling/notes.md`](../07-Domain-Modeling/notes.md).

---

## Stringly-Typed Design

**Smell**: strings standing in for a closed set of values.

```csharp
order.SetStatus("PAID");   // typo "PIAD" compiles and fails at runtime
```

**Fix**: `enum` for simple closed sets; a state class or type hierarchy
when each value carries behavior.

---

## Boolean Parameter Explosion

**Smell**: call sites nobody can read.

```csharp
CreateOrder(true, false, true, false);
```

**Why it hurts**: you must open the method signature to understand any call,
and swapping two flags is an invisible bug.

**Fix**: named arguments as a minimum (`CreateOrder(isGift: true, ...)`),
an options object, or a
[Builder](../04-Design-Patterns/Creational/Builder/notes.md). A boolean
parameter that selects between two behaviors often means the method should
be two methods.

---

## Leaky Encapsulation

**Smell**: exposing mutable internal collections.

```csharp
public List<OrderLineItem> Items { get; set; }  // anyone can .Clear()
```

**Fix**: private backing field, expose `IReadOnlyList<T>`, mutate only
through methods that enforce rules.

---

## Deep Inheritance Hierarchies

**Smell**: four-plus levels of `class X : Y : Z : W`, or inheriting purely
to reuse a couple of fields.

**Why it hurts**: behavior is scattered across the chain, base changes
break distant subclasses, and you get locked into a single axis of
variation.

**Fix**: favor composition. Two independent axes of variation is
[Bridge](../04-Design-Patterns/Structural/Bridge/notes.md); optional
stackable behavior is
[Decorator](../04-Design-Patterns/Structural/Decorator/notes.md).

---

## Premature Abstraction / Speculative Generality ⭐

**Smell**: interfaces with one implementation, plugin architectures with
one plugin, config for things that never change, `IFooFactoryProvider`.

**Why it hurts**: every layer is indirection a reader must traverse to find
out that nothing varies. It's cost with no benefit, and it makes the design
*look* like it anticipates variation that doesn't exist.

**Fix**: YAGNI. Extract the interface when the second implementation
actually arrives — that refactor is cheap and your IDE does it for you.

**Interview note**: this is the anti-pattern candidates most often commit
*while trying to impress*. Saying "I wouldn't abstract this yet" is a
stronger answer than adding the interface.

---

## Pattern Overuse ("Patternitis")

**Smell**: a Factory producing a Builder that returns a Strategy wrapped in
a Decorator behind a Facade — for a class with two fields.

**Fix**: each pattern must be justified by a concrete problem *in this
design*. If you can't name the problem, remove the pattern. See the
[Pattern Selection Guide](../04-Design-Patterns/Pattern-Selection-Guide.md).

---

## Singleton Abuse

**Smell**: Singletons used as a convenience to avoid passing references;
`Foo.Instance` called from deep inside business logic.

**Why it hurts**: hidden dependencies (the constructor doesn't declare
them), global mutable state, and tests that leak into each other because
the instance persists across them.

**Fix**: create one instance at the composition root and **inject** it. You
keep "exactly one instance" without the global access point. See
[Singleton](../04-Design-Patterns/Creational/Singleton/notes.md).

---

## Shotgun Surgery

**Smell**: one small requirement change forces edits across many files —
"add a vehicle type" means touching a factory, three switch statements, a
DTO, and a validator.

**Why it hurts**: it's the practical cost of a missing abstraction, and
it's how bugs get introduced (you'll miss one of the five places).

**Fix**: find the axis of change and give it one home — usually
polymorphism replacing scattered switches (OCP).

---

## Circular Dependencies

**Smell**: `A` references `B`, which references `A`.

**Why it hurts**: neither can be understood, tested, or reused
independently, and lifecycle/initialization order becomes fragile.

**Fix**: extract the shared concept into a third type, invert one direction
with an interface (DIP), or introduce a
[Mediator](../04-Design-Patterns/Behavioral/Mediator/notes.md) if many
peers are involved.

---

## Quick self-audit for your own designs

Before you say "I'm done" in an interview, scan for:

- [ ] Any class with a name ending in `Manager`/`Helper`/`Util`?
- [ ] Any class that's only properties, with its logic living elsewhere?
- [ ] Any public mutable collection?
- [ ] Any `switch` you'd have to revisit for a new requirement?
- [ ] Any interface with exactly one implementation and no second in sight?
- [ ] Any pattern you can't justify with a concrete problem?
- [ ] Any important concept still a bare `string` or `decimal`?
- [ ] Any method with 3+ boolean parameters?

Catching one of these yourself and fixing it aloud is worth more than a
design that quietly avoided them.
