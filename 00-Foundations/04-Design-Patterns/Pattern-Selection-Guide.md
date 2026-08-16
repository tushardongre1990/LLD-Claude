# Pattern Selection Guide

**The rule that matters more than any table below:**

> A pattern is the *result* of identifying a design problem, never the
> starting point. You notice a specific pain in your design, then reach
> for the pattern that relieves it. Deciding "I'll use Observer" before
> you know what's varying is how designs get over-engineered — and
> interviewers spot it immediately.

So use this page as a **symptom → candidate** lookup while designing, not
as a checklist of patterns to work in.

## Symptom → candidate pattern

| What you notice in your design | Likely pattern | Why |
|---|---|---|
| Behavior varies by **algorithm/rule**, and the caller picks it | [Strategy](Behavioral/Strategy/notes.md) | Interchangeable algorithms behind one interface |
| Behavior varies by **the object's own lifecycle phase**, and it changes itself | [State](Behavioral/State/notes.md) | Each phase is a class that owns its legal transitions |
| A growing `switch`/`if-else` deciding **which class to build** | [Simple Factory / Factory Method](Creational/FactoryMethod/notes.md) | Centralizes (or with the creator hierarchy, eliminates) the type decision |
| Several related objects must be **created as a matched set** | [Abstract Factory](Creational/AbstractFactory/notes.md) | Keeps a product family mutually consistent |
| A constructor with **many optional parameters**, or you want an immutable result | [Builder](Creational/Builder/notes.md) | Step-by-step, readable, self-documenting construction |
| Creating an object is **expensive**, and a configured template already exists | [Prototype](Creational/Prototype/notes.md) | Clone instead of rebuild |
| Exactly **one instance** must coordinate the system | [Singleton](Creational/Singleton/notes.md) | …but prefer DI of a single instance; see that page's warnings |
| **Many objects must react** when one thing changes | [Observer](Behavioral/Observer/notes.md) | One-to-many notification without the subject knowing concrete types |
| A third-party/legacy API **doesn't match** the interface you need | [Adapter](Structural/Adapter/notes.md) | Translates one interface into another |
| Callers must orchestrate **many services in a fixed dance** | [Facade](Structural/Facade/notes.md) | One simple entry point over a complex subsystem |
| You'd need a class per **combination** of optional behaviors | [Decorator](Structural/Decorator/notes.md) | Stack behaviors at runtime instead of subclassing combinatorially |
| You need to **control access** (lazy-load, permission-check, cache, log) | [Proxy](Structural/Proxy/notes.md) | Same interface, gated/deferred |
| The data is a **tree**, and you're writing `if (isLeaf)` everywhere | [Composite](Structural/Composite/notes.md) | Leaves and containers share one interface |
| **Two dimensions** of variation are heading toward an N×M class explosion | [Bridge](Structural/Bridge/notes.md) | Split the hierarchies, compose them |
| A **huge number** of similar objects is a memory problem | [Flyweight](Structural/Flyweight/notes.md) | Share the intrinsic state; pass the extrinsic state in |
| You need **undo/redo**, queuing, or logging of actions | [Command](Behavioral/Command/notes.md) | Requests become objects with `Execute`/`Undo` |
| You need to **snapshot and restore** an object's state | [Memento](Behavioral/Memento/notes.md) | Restore a whole prior state without exposing internals |
| A request should try **several handlers in order** | [Chain of Responsibility](Behavioral/ChainOfResponsibility/notes.md) | Handlers decide to process or forward |
| Several classes share an **algorithm skeleton**, differing in steps | [Template Method](Behavioral/TemplateMethod/notes.md) | Sequence written once in a base class |
| Many peers have **tangled many-to-many references** | [Mediator](Behavioral/Mediator/notes.md) | Route interaction through one coordinator |
| You need to traverse a **custom collection** without exposing its internals | [Iterator](Behavioral/Iterator/notes.md) | Usually just `IEnumerable<T>` in C# |
| **Operations** over a stable class hierarchy keep growing | [Visitor](Behavioral/Visitor/notes.md) | New operation = new visitor, no element edits |

## Before you commit to a pattern, ask three questions

1. **What exactly varies here, and is it likely to vary again?** If the
   answer is "nothing yet, but maybe someday," that's YAGNI — don't add
   the abstraction. See
   [`../06-Core-Design-Principles/notes.md`](../06-Core-Design-Principles/notes.md).
2. **Does the pattern actually remove the pain, or just relocate it?** A
   Simple Factory still has a switch; it has centralized the change, not
   eliminated it. Be able to say which one you achieved.
3. **What's the cost?** Every pattern adds indirection — more classes, more
   files, more hops to read. That cost is worth paying when the variation
   is real, and pure overhead when it isn't.

## The strongest thing you can say in an interview

When an interviewer asks "could you use a pattern here?", a candidate who
says *"I could, but I don't think it's justified — the current requirements
only have one pricing rule, so I'd keep it a plain method and extract a
Strategy the moment a second rule appears"* scores **higher** than one who
adds the interface reflexively. Knowing when *not* to apply a pattern is a
seniority signal.
