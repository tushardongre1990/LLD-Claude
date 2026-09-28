# Study Plan — what to read before each case study

One path through the vault: **read a little → solve a case study → read a
little → solve the next.** Foundation topics are read *just before* the first
case study that needs them, never in bulk.

Assumes [01-OOP-Basics](00-Foundations/01-OOP-Basics/notes.md),
[02-UML](00-Foundations/02-UML-Object-Oriented-Design/notes.md) and
[03-SOLID](00-Foundations/03-SOLID-Principles/notes.md) are done.

## How to use each entry

- **Read before** — new material only. Everything from earlier entries is
  assumed. Sections are named by their heading in that file.
- **Revisit** — something you've already read that this problem leans on hard.
- **Learn during** — concepts the vault deliberately does *not* cover as a
  topic; you pick them up inside the case study itself.
- **After solving** — a [Pattern-Comparisons](00-Foundations/04-Design-Patterns/Pattern-Comparisons.md)
  entry to read *after* your attempt, while the design is fresh. These are
  classic "X vs Y?" interview questions.
- Patterns listed are **candidates you should be able to evaluate**, not
  patterns you must use. Deciding against one, with a reason, is a valid
  answer.
- Run `dotnet run --project Runner <name>` for any pattern with a demo.

The order is a suggestion: it keeps each step to 0–3 new things and pulls the
⭐ studies (highest interview frequency) earlier than the roadmap numbering.
The `#` is the roadmap number in [README.md](README.md).

---

## Stage 1 — first design

### ☐ #1 Parking Lot
**Read before**
- [05-Interview-Approach](00-Foundations/05-Interview-Approach/notes.md) — **all of it**; it's the procedure you run on every case study
- [07-Domain-Modeling](00-Foundations/07-Domain-Modeling/notes.md) — *Entity vs Value Object*, *Invariants*, *Putting it together: how to read a prompt*
- [06-Core-Design-Principles](00-Foundations/06-Core-Design-Principles/notes.md) — *KISS*, *YAGNI*, *Composition over Inheritance*, *Encapsulate What Varies*
- [Design Patterns index](00-Foundations/04-Design-Patterns/README.md) + [Pattern-Selection-Guide](00-Foundations/04-Design-Patterns/Pattern-Selection-Guide.md)
- [Strategy](00-Foundations/04-Design-Patterns/Behavioral/Strategy/notes.md) (`strategy`)
- [Factory Method](00-Foundations/04-Design-Patterns/Creational/FactoryMethod/notes.md) (`factory`)
- Before writing code: [09-Testing](00-Foundations/09-Testing/notes.md) — *AAA*, *What to test in an LLD case study*, *Test the failure paths*

**After solving**
- [Singleton](00-Foundations/04-Design-Patterns/Creational/Singleton/notes.md) (`singleton`) — so you can answer the near-certain follow-up "should `ParkingLot` be a Singleton?" (usually: no, prefer one injected instance)

---

## Stage 2 — lifecycles and state machines

### ☐ #2 Vending Machine
**Read before**
- [State](00-Foundations/04-Design-Patterns/Behavioral/State/notes.md) (`state`)
- [09-Testing](00-Foundations/09-Testing/notes.md) — *Example: testing the State pattern*

**Learn during** — change-making (greedy vs. when greedy fails)

**After solving** — *Strategy vs State* ⭐

### ☐ #4 Elevator
**Read before** — nothing new. This one tests whether State and Strategy stuck.

**Revisit** — [Strategy](00-Foundations/04-Design-Patterns/Behavioral/Strategy/notes.md) (dispatch algorithm)

**Learn during** — request scheduling (nearest-car, SCAN/LOOK)

Keep it single-threaded for now; its concurrency level comes back after Stage 5.

---

## Stage 3 — requests, commands, transactions

### ☐ #3 ATM
**Read before**
- [Chain of Responsibility](00-Foundations/04-Design-Patterns/Behavioral/ChainOfResponsibility/notes.md) (`chain`)
- [Command](00-Foundations/04-Design-Patterns/Behavioral/Command/notes.md) (`command`)
- [07-Domain-Modeling](00-Foundations/07-Domain-Modeling/notes.md) — *Aggregate and Aggregate Root*, *Money*, *Application Service*
- [06-Core-Design-Principles](00-Foundations/06-Core-Design-Principles/notes.md) — *Tell, Don't Ask*, *Law of Demeter*, *Fail Fast*

**After solving** — *Command vs Strategy*

Transaction atomicity: design it single-threaded now; revisit after Stage 5.

---

## Stage 4 — notifications, repositories, time

### ☐ #5 Library Management
**Read before**
- [Observer](00-Foundations/04-Design-Patterns/Behavioral/Observer/notes.md) (`observer`)
- [07-Domain-Modeling](00-Foundations/07-Domain-Modeling/notes.md) — *Dates, times, and durations*, *Domain Service*, *Repository* (07 is now complete)
- [06-Core-Design-Principles](00-Foundations/06-Core-Design-Principles/notes.md) — the rest: *DRY*, *Program to an Interface*, *High Cohesion, Low Coupling*, *Principle conflicts* (06 is now complete)
- [10-Anti-Patterns](00-Foundations/10-Anti-Patterns/notes.md) — *God Object*, *Anemic Domain Model*, *Primitive Obsession*, *Premature Abstraction*, *Quick self-audit* → **run the self-audit on your first four designs**

**After solving** — *Observer vs Pub-Sub*

### ☐ #6 Amazon Locker
**Read before** — nothing new.

**Revisit** — State, Strategy (size/fit assignment), *Dates, times, and durations* (expiry)

### ☐ #7 Meeting Scheduler
**Read before** — nothing new.

**Revisit** — *Dates, times, and durations*

**Learn during** — interval overlap with half-open ranges (`startA < endB && startB < endA`), recurring events, time zones

---

## Stage 5 — concurrency

### ☐ #8 Movie Ticket Booking ⭐
**Read before**
- [08-Concurrency](00-Foundations/08-Concurrency/notes.md) — **all of it** (`concurrency`); slow down on *Optimistic vs Pessimistic* and *Seat/resource locking with a timeout*
- [09-Testing](00-Foundations/09-Testing/notes.md) — revisit *Test the failure paths*; concurrency tests must hit the losing path, not just the happy one

**Learn during** — hold-with-timeout, idempotency / retry (deliberately taught here, not as a separate topic)

**Then go back** and add the concurrency level to Parking Lot (two gates, one spot), ATM (atomic withdrawal), and Elevator (request queue).

### ☐ #23 Cab Booking / Ride Sharing
**Read before** — nothing required.

**Revisit** — 08-Concurrency *Lock granularity*, *Deadlock and lock ordering* (driver double-assignment)

**Learn during** — matching, geo-indexing (at the level of "which data structure would you use")

**After solving**
- [Mediator](00-Foundations/04-Design-Patterns/Behavioral/Mediator/notes.md) (`mediator`) — often *listed* for Cab Booking but genuinely optional; read it so you can argue why you did or didn't use it
- *Observer vs Mediator* ⭐

---

## Stage 6 — money and ledgers

### ☐ #22 Splitwise ⭐
**Read before** — nothing new.

**Revisit** — 07 *Money* and *Invariants* (this problem is mostly about them)

**Learn during** — rounding and allocating remainders, debt simplification

### ☐ #9 Online Stock Brokerage
**Read before** — nothing new.

**Revisit** — Command, State (order lifecycle), Observer (price feed), *Money*

**Learn during** — order matching, partial fills

---

## Stage 7 — games and undo

### ☐ #15 Chess ⭐
**Read before**
- [Memento](00-Foundations/04-Design-Patterns/Behavioral/Memento/notes.md) (`memento`)

**Revisit** — Command (move history)

**Learn during** — polymorphic move validation, check/checkmate detection

**After solving** — *Command vs Memento (for undo)*

### ☐ #14 Online Blackjack
**Read before**
- [Template Method](00-Foundations/04-Design-Patterns/Behavioral/TemplateMethod/notes.md) (`template`) — candidate for the turn/round skeleton

**Revisit** — Factory Method (deck/card creation — decide whether it earns its place)

**After solving** — *Template Method vs Strategy* ⭐

---

## Stage 8 — wrapping and building

### ☐ #10 Car Rental
**Read before**
- [Decorator](00-Foundations/04-Design-Patterns/Structural/Decorator/notes.md) (`decorator`) — add-ons

**Revisit** — *Dates, times, and durations* (availability over date ranges)

### ☐ #11 Hotel Management
**Read before** — nothing new.

**Revisit** — 07 *Aggregate and Aggregate Root*; 08-Concurrency *Multi-resource booking without simultaneous locks* (double booking across nights)

### ☐ #12 Restaurant Management
**Read before**
- [Builder](00-Foundations/04-Design-Patterns/Creational/Builder/notes.md) (`builder`) — candidate for order construction

**Revisit** — State (order/table), 08-Concurrency (table assignment)

**After solving** — *Factory vs Builder*

### ☐ #13 Airline Management
**Read before** — nothing new.

**Revisit** — 08-Concurrency *Multi-resource booking without simultaneous locks* (multi-leg itineraries), Builder

### ☐ #16 Amazon Online Shopping
**Read before**
- [Facade](00-Foundations/04-Design-Patterns/Structural/Facade/notes.md) (`facade`) — checkout
- [Adapter](00-Foundations/04-Design-Patterns/Structural/Adapter/notes.md) (`adapter`) — third-party payment SDK

**Revisit** — 07 *Application Service*, *Money*; 08-Concurrency (inventory)

**Learn during** — idempotent checkout / payment retries

**After solving** — *Adapter vs Facade*, *Adapter vs Decorator*, *Mediator vs Facade*

---

## Stage 9 — trees and graphs

### ☐ #17 Stack Overflow
**Read before**
- [Composite](00-Foundations/04-Design-Patterns/Structural/Composite/notes.md) (`composite`) — comment threads

**Revisit** — 08-Concurrency (vote integrity)

**After solving** — *Composite vs Decorator*

### ☐ #18 Facebook
**Read before**
- [Proxy](00-Foundations/04-Design-Patterns/Structural/Proxy/notes.md) (`proxy`) — candidate for privacy/access checks

**Learn during** — graph modelling (users and relationships), privacy rules; feed at scale is HLD, say so and stop

**After solving** — *Decorator vs Proxy* ⭐, *Proxy vs Facade*

### ☐ #19 ESPNcricinfo
**Read before** — nothing new.

**Revisit** — Observer (live updates), State (match/innings), Command (event replay)

### ☐ #20 LinkedIn
**Read before** — nothing new.

**Revisit** — Observer (notifications)

**Learn during** — connection-degree queries (BFS over a graph)

### ☐ #21 Jigsaw Puzzle
**Read before** — nothing new.

**Revisit** — Strategy (solver)

**Learn during** — edge matching and its efficiency

---

## Before your first real interview

- [ ] [10-Anti-Patterns](00-Foundations/10-Anti-Patterns/notes.md) — the rest of it
- [ ] [Pattern-Comparisons](00-Foundations/04-Design-Patterns/Pattern-Comparisons.md) — all of it, ending with *Quick self-test*
- [ ] Remaining patterns, at **recognize-and-explain** depth only:
  [Abstract Factory](00-Foundations/04-Design-Patterns/Creational/AbstractFactory/notes.md) (+ *Factory Method vs Abstract Factory* ⭐),
  [Prototype](00-Foundations/04-Design-Patterns/Creational/Prototype/notes.md) (+ *Prototype vs Factory*),
  [Bridge](00-Foundations/04-Design-Patterns/Structural/Bridge/notes.md) (+ *Bridge vs Strategy vs Adapter*, *Bridge vs Adapter*),
  [Flyweight](00-Foundations/04-Design-Patterns/Structural/Flyweight/notes.md),
  [Iterator](00-Foundations/04-Design-Patterns/Behavioral/Iterator/notes.md),
  [Visitor](00-Foundations/04-Design-Patterns/Behavioral/Visitor/notes.md),
  [Interpreter](00-Foundations/04-Design-Patterns/Behavioral/Interpreter/notes.md)
- [ ] Redo two or three ⭐ studies as a **timed 45-minute mock, no notes, narrating aloud**
