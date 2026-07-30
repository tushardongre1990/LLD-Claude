# Bridge

**Category**: Structural
**Intent**: Decouple an abstraction from its implementation so the two can
**vary independently** — instead of a combinatorial class hierarchy for
every abstraction x implementation pairing.

## The problem: a second axis of variation blows up inheritance

Modeling "remote controls x devices" with pure inheritance
(`BasicRemote`, `AdvancedRemote`) x (`TV`, `Radio`) needs a class for every
pairing: `BasicTvRemote`, `AdvancedTvRemote`, `BasicRadioRemote`,
`AdvancedRadioRemote`... — the same combinatorial explosion problem
Decorator solves for "add-ons," but here it's two independent hierarchies
that both want to grow.

## Structure

```mermaid
classDiagram
    class RemoteControl {
        <<abstract>>
        #Device device
        +TogglePower() void
    }
    class BasicRemote
    class AdvancedRemote

    class Device {
        <<interface>>
        +IsOn() bool
        +TurnOn() void
        +TurnOff() void
    }
    class Tv
    class Radio

    RemoteControl <|-- BasicRemote
    RemoteControl <|-- AdvancedRemote
    RemoteControl o-- Device : bridge
    Device <|.. Tv
    Device <|.. Radio
```

`RemoteControl` (the **abstraction** hierarchy) holds a reference to a
`Device` (the **implementation** hierarchy) instead of inheriting from it.
Any `RemoteControl` subtype can be paired with any `Device` subtype at
runtime — `new AdvancedRemote(new Radio())` — with zero new classes.

## When to use

- You have (or foresee) **two independent dimensions of variation** and
  don't want their cross-product as concrete classes.
- You want to swap the implementation at runtime without touching the
  abstraction's code (or vice versa).

## Bridge vs Strategy vs Adapter — all "hold a reference to an interface," different intent

| | Bridge | Strategy | Adapter |
|---|---|---|---|
| Intent | Split **two hierarchies** that both grow, so they vary independently | Swap **one algorithm** at runtime | Make an **existing incompatible interface** fit what you need |
| When decided | Usually designed in upfront | Usually designed in upfront | Usually added reactively, for integration |

Structurally these three can look almost identical in a class diagram (a
class holding an interface reference) — the *intent* is what you should
lead with in an interview answer, not the shape.

## Interview variations

- "We have remote types (basic/advanced/voice) and device types
  (TV/radio/speaker) and both keep growing — how do you avoid N×M
  classes?" → Bridge.
- "How is this different from Strategy?" → Bridge is about decoupling two
  hierarchies that each have multiple concrete types and evolve
  independently; Strategy is about swapping a single algorithm/behavior
  used by one class.
