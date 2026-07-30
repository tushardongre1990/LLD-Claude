# Strategy

**Category**: Behavioral
**Intent**: Define a family of interchangeable algorithms, encapsulate each
one, and make them swappable at runtime — the class using the algorithm is
decoupled from which specific one it's using.

This is **the single most-used pattern in LLD interviews**. Nearly every
case study has at least one Strategy: fare calculation, discount rules,
sorting/matching logic, parking fee rules, route-finding.

## Structure

```mermaid
classDiagram
    class FeeStrategy {
        <<interface>>
        +Calculate(hours) decimal
    }
    class HourlyFeeStrategy
    class FlatDayRateStrategy
    class FreeFirstHourStrategy
    FeeStrategy <|.. HourlyFeeStrategy
    FeeStrategy <|.. FlatDayRateStrategy
    FeeStrategy <|.. FreeFirstHourStrategy

    class ParkingTicket {
        -FeeStrategy strategy
        +CalculateFee(hours) decimal
    }
    ParkingTicket o-- FeeStrategy
```

`ParkingTicket` doesn't know or care *how* the fee is computed — it just
calls `_strategy.Calculate(hours)`. Swapping pricing schemes (hourly, flat
daily rate, first-hour-free promo) means adding a new class, never editing
`ParkingTicket`. This is the pattern-shaped version of the OCP fix shown in
`03-SOLID-Principles/notes.md`.

## When to use

- A behavior has **multiple interchangeable variants**, selected at
  runtime (by config, user choice, or context) — and you want to avoid a
  `switch`/`if-else` chain choosing between them inline.
- You want each variant unit-testable in isolation.

## Strategy vs State — the other big mix-up

Structurally near-identical (a class holds an interface reference, swaps
implementations). The difference is **who decides to swap, and why**:

| | Strategy | State |
|---|---|---|
| Who picks the implementation | The **client/caller**, usually once, based on configuration | The **object itself**, transitioning automatically as things happen |
| Purpose | Interchangeable **algorithms** doing the same conceptual job | Modeling an object's **lifecycle**, where behavior legitimately differs by phase |
| Example | Pick a `FeeStrategy` when the ticket is created | An `Order` moves Placed → Shipped → Delivered on its own, each state limiting what's allowed next |

See `../State/notes.md` for the State-pattern version of this comparison.

## Interview variations

- "How would you support multiple pricing schemes (hourly / flat-rate /
  membership) without a `switch` on an enum?" → Strategy, straight OCP
  motivation.
- "What's the difference between Strategy and State?" (see table above —
  asked constantly, know it cold).
- "How would a client choose which strategy to use?" → constructor
  injection, a factory, or configuration — tie back to DIP.
