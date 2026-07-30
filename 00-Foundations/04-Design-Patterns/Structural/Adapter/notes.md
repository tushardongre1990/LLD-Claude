# Adapter

**Category**: Structural
**Intent**: Convert the interface of a class into another interface clients
expect, letting incompatible interfaces work together without modifying
either side.

## Structure

```mermaid
classDiagram
    class IPaymentGateway {
        <<interface>>
        +Charge(amountCents) bool
    }
    class LegacyStripeSdk {
        +MakePayment(dollars) string
    }
    class StripeAdapter {
        -LegacyStripeSdk sdk
        +Charge(amountCents) bool
    }
    IPaymentGateway <|.. StripeAdapter
    StripeAdapter --> LegacyStripeSdk : wraps
```

`StripeAdapter` implements the interface your app already expects
(`IPaymentGateway`) and internally translates calls to whatever shape the
third-party SDK actually has (`MakePayment(dollars)`, different units,
different return type). Your app code never touches `LegacyStripeSdk`
directly.

## When to use

- Integrating a **third-party library or legacy code** whose interface
  doesn't match what the rest of your system expects, and you can't (or
  shouldn't) modify that external code.
- Migrating from an old interface to a new one gradually — the adapter lets
  old and new coexist.

## Object Adapter vs Class Adapter

- **Object Adapter** (shown above): wraps an *instance* of the adaptee via
  composition. Works in C#/TypeScript (no multiple inheritance needed) —
  this is the version you'll actually write.
- **Class Adapter**: inherits from the adaptee directly. Requires multiple
  inheritance, so it isn't idiomatic in C# or TypeScript — mention it exists
  (from the original GoF book, using C++) but default to Object Adapter.

## Adapter vs Facade — a common mix-up

| | Adapter | Facade |
|---|---|---|
| Purpose | Make an **incompatible** interface match what you expect | Simplify a **complex but compatible** subsystem |
| Interface count | Wraps one interface into another, same-ish granularity | Collapses many interfaces into one simpler one |
| Motivation | You *have to* — integration constraint | You *choose to* — for usability |

## Interview variations

- "We're integrating a third-party SDK with a totally different method
  signature than our internal `IPaymentGateway` — how do you keep the rest
  of the codebase clean?" → Adapter, by name, with the wrapping diagram.
- "What's the difference between Adapter and Facade?" (see table above).
