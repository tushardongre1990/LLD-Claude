# Pattern Comparisons

"What's the difference between X and Y?" is one of the most reliably-asked
LLD interview questions, because many patterns are **structurally similar
and differ only in intent**. This page collects every pair worth knowing.

**The meta-answer that works for almost all of these**: lead with
*intent*, not structure. Several of these pairs draw an identical class
diagram — what separates them is the problem they're solving and who
decides what.

---

## Strategy vs State ⭐ (asked most often)

Both: a context object holds an interface reference and delegates to it.

| | Strategy | State |
|---|---|---|
| Who picks the implementation | The **client**, usually once at construction | The **object itself**, as events happen to it |
| Do implementations know each other? | No — strategies are independent | Yes — each state knows which states it can transition to |
| Models | Interchangeable **algorithms** | A **lifecycle** with legal/illegal transitions |
| Example | `FeeStrategy` chosen when a ticket is created | `Order`: Placed → Paid → Shipped → Delivered |

**One-liner**: Strategy swaps *how a job is done*; State swaps *what the
object currently is*.

---

## Adapter vs Facade

| | Adapter | Facade |
|---|---|---|
| Problem | An interface is **incompatible** with what you need | A subsystem is **compatible but complicated** |
| Motivation | You *must* — an integration constraint | You *choose to* — for caller convenience |
| Shape | Wraps one thing into a different interface | Collapses many classes behind one simpler entry point |

**One-liner**: Adapter changes an interface's *shape*; Facade reduces a
subsystem's *surface*.

---

## Adapter vs Decorator

| | Adapter | Decorator |
|---|---|---|
| Interface after wrapping | **Different** from the wrapped object's | **Same** as the wrapped object's |
| Adds behavior? | No — translates | Yes — layers on new responsibility |
| Stackable? | Not meaningfully | Yes, that's the point |

---

## Decorator vs Proxy ⭐

Nearly identical structure — both wrap an object behind its own interface.

| | Decorator | Proxy |
|---|---|---|
| Intent | **Add** responsibility | **Control access** to the same conceptual object |
| Caller-visible capability | New behavior the base lacked | Unchanged contract, possibly deferred/gated |
| Who supplies the wrapped object | Caller passes in an existing instance | Proxy often creates/owns it (lazily) |
| Stacking | Designed for it | Rare |

**One-liner**: Decorator makes it *do more*; Proxy decides *whether and
when* it does it at all.

---

## Composite vs Decorator

Both are recursive and share a base type with what they contain.

| | Composite | Decorator |
|---|---|---|
| Children | **Many** — models a tree | **Exactly one** — models a wrapping chain |
| Models | Whole-part containment (folder contains files) | Layered behavior on a single object |

---

## Factory Method vs Abstract Factory ⭐

| | Factory Method | Abstract Factory |
|---|---|---|
| Produces | **One** product | A **family** of related products |
| Shape | One overridable method in a creator hierarchy | An interface with several creation methods |
| Varies by | Which creator **subclass** you use | Which concrete **factory object** you inject |

Also see [Simple Factory vs Factory Method](Creational/FactoryMethod/notes.md)
— a distinct and equally common question, since "Simple Factory" isn't a
GoF pattern at all.

---

## Factory vs Builder

| | Factory | Builder |
|---|---|---|
| Answers | "**Which class** do I instantiate?" (a decision) | "**How do I assemble** this object?" (a process) |
| Call shape | One call, returns a finished object | Several chained calls, then `Build()` |
| Best when | Type varies | Many optional parts / immutability wanted |

---

## Command vs Strategy

| | Command | Strategy |
|---|---|---|
| Encapsulates | An **action to perform** (often with its receiver + args bound in) | An **algorithm** the context runs |
| Typically supports | Undo, queuing, logging, replay | Just being called |
| Lifetime | Often stored in a history/queue | Usually held for the context's lifetime |

---

## Command vs Memento (for undo)

| | Command | Memento |
|---|---|---|
| Undo works by | **Reversing the action** (`Undo()` knows the inverse) | **Restoring a snapshot** of prior state |
| Memory cost | Low — store the delta | Higher — store whole states |
| Best when | Actions have clean inverses | State is complex/hard to invert |

They combine well: Command for the action log, Memento when a particular
command can't cleanly invert itself.

---

## Observer vs Mediator ⭐

| | Observer | Mediator |
|---|---|---|
| Topology | **One-to-many** broadcast from a subject | **Many-to-many** routed through a hub |
| Direction | One-way: subject → observers | Multi-directional between peers |
| Do participants know the hub? | Observers know the subject they subscribe to | Colleagues are designed around the mediator |

A Mediator often *uses* Observer internally to push updates.

---

## Observer vs Pub-Sub

| | Observer (GoF) | Pub-Sub |
|---|---|---|
| Coupling | Subject holds direct observer references | A broker sits between; neither side knows the other |
| Scope | In-process | Often distributed across services |
| Delivery | Synchronous, inline | Usually async/queued |

---

## Mediator vs Facade

| | Mediator | Facade |
|---|---|---|
| Direction | Two-way, among peers | One-way, caller → subsystem |
| Subsystem awareness | Colleagues know the mediator | Subsystem classes don't know the facade exists |

---

## Template Method vs Strategy ⭐

| | Template Method | Strategy |
|---|---|---|
| Mechanism | **Inheritance** — override steps | **Composition** — inject a whole algorithm |
| Granularity | Individual **steps** inside a fixed sequence | The **entire** algorithm |
| Bound at | Compile time (which subclass) | Runtime (which object) |

**One-liner**: Template Method varies *steps within* an algorithm;
Strategy varies *the algorithm*.

---

## Bridge vs Strategy vs Adapter

All three hold an interface reference — the diagrams look the same.

| | Bridge | Strategy | Adapter |
|---|---|---|---|
| Intent | Let **two hierarchies** evolve independently | Swap **one algorithm** | Make an **existing incompatible** interface fit |
| Both sides have multiple types? | Yes, by design | Usually only the strategy side | No — one specific adaptee |
| When decided | Designed in upfront | Designed in upfront | Added reactively, for integration |

---

## Bridge vs Adapter

| | Bridge | Adapter |
|---|---|---|
| When applied | **Before** the code exists — a design decision | **After** — you're stuck with an existing interface |
| Goal | Prevent an N×M explosion | Make two things interoperate |

---

## Proxy vs Facade

| | Proxy | Facade |
|---|---|---|
| Interface | **Same** as the object it fronts | **New, simpler** one over many objects |
| Fronts | Exactly one object | A whole subsystem |

---

## Prototype vs Factory

| | Prototype | Factory |
|---|---|---|
| Creates by | **Cloning** an existing configured instance | **Constructing** from scratch |
| Best when | A rich template already exists and copying is cheaper | The type decision is the hard part |

---

## Quick self-test

Cover the right-hand columns and answer these out loud:

1. Strategy vs State — who decides the swap?
2. Decorator vs Proxy — same interface in both; what differs?
3. Factory Method vs Abstract Factory — one product or many?
4. Template Method vs Strategy — inheritance or composition?
5. Observer vs Mediator — what's the topology of each?
6. Adapter vs Bridge — which is reactive and which is planned?
7. Composite vs Decorator — how many children does each hold?
