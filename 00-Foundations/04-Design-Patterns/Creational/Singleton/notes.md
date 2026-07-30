# Singleton

**Category**: Creational
**Intent**: Ensure a class has exactly one instance, and provide a single
global access point to it.

## Structure

```mermaid
classDiagram
    class Singleton {
        -static Singleton instance
        -Singleton()
        +static GetInstance() Singleton
    }
```

- Constructor is `private` — nobody outside can `new` it.
- A static field holds the one instance.
- A static accessor creates it on first use (lazy) and returns the same
  instance afterward.

## When to use

- Exactly one instance must coordinate actions across the system —
  a `ParkingLot` (there's physically one lot), a config/settings object, a
  logger, a connection pool manager, an ID generator.

## When NOT to use (this is what interviewers actually probe)

- **It's a global variable in disguise** — overuse makes unit testing hard
  (hidden shared state between tests) and hides dependencies (a class using
  `Singleton.GetInstance()` internally doesn't declare that dependency in its
  constructor, violating the spirit of DIP).
- If you find yourself reaching for Singleton just to avoid passing a
  reference around, prefer **dependency injection** instead: construct one
  instance at the composition root and pass it in. Same "one instance"
  outcome, but testable and explicit.
- In an interview, saying *"I'll make ParkingLot a Singleton, but I'd inject
  it rather than have every class call `GetInstance()` directly"* signals
  real experience, not just pattern-name recall.

## Thread safety

A naive lazy Singleton is **not thread-safe** — two threads can both pass
the `if (instance == null)` check before either assigns it, creating two
instances. This is a very common interview follow-up ("is this thread-safe?
how would you fix it?"). Fixes, in order of how interviewers like to see it:

1. **Static initialization** (eager) — CLR guarantees thread-safe, one-time
   initialization of static fields. Simplest fix if you don't need lazy
   creation.
2. **`Lazy<T>`** in C# — thread-safe by default, still lazy.
3. **Double-checked locking** — the classic manual approach, more code, only
   worth it if you can't use `Lazy<T>` for some reason (e.g. pre-.NET, or
   the constructor has expensive side effects you want tightly controlled).

See `Singleton.cs` for all three implemented side by side.

## Interview variations

- "Is your Singleton thread-safe? Prove it." → walk through the race
  condition, then show `Lazy<T>` or double-checked locking.
- "How would you unit test a class that depends on a Singleton?" → inject an
  interface instead of calling `GetInstance()` directly inside the
  dependent class; the Singleton can still enforce "one instance" at the
  composition root.
- "What's wrong with Singletons?" → global mutable state, hidden
  dependencies, hard to test, hard to subclass/replace. Know the criticism,
  don't just defend the pattern blindly.
