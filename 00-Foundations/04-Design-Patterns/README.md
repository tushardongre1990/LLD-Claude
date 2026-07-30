# Design Patterns — Index

Design patterns are named solutions to recurring design problems. In an LLD
interview they matter for two reasons:

1. **Vocabulary/speed** — saying "I'll use Strategy here" communicates an
   entire structure in three words instead of you re-deriving it live.
2. **They're the concrete payoff of SOLID** — every pattern here exists
   because it satisfies OCP, DIP, or SRP in a specific recurring shape. If
   you understand *why* (see `00-Foundations/03-SOLID-Principles`), you'll
   never misapply one.

**Anti-pattern to avoid in interviews**: don't force a pattern in just to
namedrop it. Interviewers notice "resume-driven design." Only reach for a
pattern once you've identified the actual problem it solves (varying
algorithm → Strategy; notifying multiple listeners → Observer; complex
multi-step construction → Builder; etc.). Every case study in
`01-Case-Studies` will call out which patterns are used and *why*, not just
that they're used.

## Cheat sheet

| Pattern | Category | Solves | Classic LLD example |
|---|---|---|---|
| [Singleton](Creational/Singleton/notes.md) | Creational | Exactly one instance, global access point | `ParkingLot` instance, config/logger |
| [Factory Method](Creational/FactoryMethod/notes.md) | Creational | Defer object creation to subclasses | Creating `Car`/`Motorcycle` from a `VehicleType` |
| [Abstract Factory](Creational/AbstractFactory/notes.md) | Creational | Create families of related objects | UI kit per theme, DB driver per vendor |
| [Builder](Creational/Builder/notes.md) | Creational | Step-by-step construction of a complex object | Building an `Order`/`Pizza`/`HttpRequest` |
| [Prototype](Creational/Prototype/notes.md) | Creational | Clone existing objects instead of rebuilding | Cloning a game board/config template |
| [Adapter](Structural/Adapter/notes.md) | Structural | Make an incompatible interface fit | Wrapping a 3rd-party payment SDK |
| [Decorator](Structural/Decorator/notes.md) | Structural | Attach behavior dynamically, without subclass explosion | Coffee add-ons, gift-wrapping an order |
| [Facade](Structural/Facade/notes.md) | Structural | Simple front door over a complex subsystem | `CheckoutFacade` over inventory/payment/shipping |
| [Composite](Structural/Composite/notes.md) | Structural | Treat individual objects and groups uniformly | File system: `File`/`Folder` tree |
| [Proxy](Structural/Proxy/notes.md) | Structural | Stand-in that controls access to the real object | Lazy-loading, access control, caching |
| [Flyweight](Structural/Flyweight/notes.md) | Structural | Share common state across many objects | Character glyphs in a text editor, map tile icons |
| [Bridge](Structural/Bridge/notes.md) | Structural | Decouple abstraction from implementation, vary both independently | Remote control (abstraction) x Device (implementation) |
| [Strategy](Behavioral/Strategy/notes.md) | Behavioral | Swap an algorithm at runtime | Fare calculation, sorting/pricing rules |
| [Observer](Behavioral/Observer/notes.md) | Behavioral | Notify many dependents when state changes | Stock ticker, seat-availability subscribers |
| [State](Behavioral/State/notes.md) | Behavioral | Behavior changes with internal state, avoid state-flag `if` chains | `Order`: Placed → Shipped → Delivered; `ElevatorState` |
| [Command](Behavioral/Command/notes.md) | Behavioral | Turn a request into an object (undo/redo/queue/log) | Remote-control buttons, `UndoManager` |
| [Chain of Responsibility](Behavioral/ChainOfResponsibility/notes.md) | Behavioral | Pass a request along a chain of handlers | Support ticket escalation, middleware pipeline |
| [Template Method](Behavioral/TemplateMethod/notes.md) | Behavioral | Fix an algorithm's skeleton, let subclasses fill in steps | Game loop, data-import pipeline |
| [Iterator](Behavioral/Iterator/notes.md) | Behavioral | Traverse a collection without exposing its internals | Custom collection traversal |
| [Mediator](Behavioral/Mediator/notes.md) | Behavioral | Centralize chatty many-to-many object communication | Chat room, air traffic control tower |
| [Memento](Behavioral/Memento/notes.md) | Behavioral | Capture/restore an object's internal state | Undo in a text editor, game save state |
| [Visitor](Behavioral/Visitor/notes.md) | Behavioral | Add new operations over a class hierarchy without editing it | Compiler AST operations, export-to-format |

## The three families, one sentence each

- **Creational** — patterns about *how objects get made* (hide `new` behind
  something smarter).
- **Structural** — patterns about *how classes/objects are composed* into
  larger structures without their code becoming tangled.
- **Behavioral** — patterns about *how objects communicate and share
  responsibility* for a behavior/algorithm.

## Suggested order to learn them

Highest interview frequency first: **Strategy, Factory Method, Singleton,
Observer, Decorator, Builder** cover the large majority of what actually
comes up in case studies. Then State, Adapter, Facade, Command. The rest
(Abstract Factory, Prototype, Composite, Proxy, Flyweight, Bridge, Chain of
Responsibility, Template Method, Iterator, Mediator, Memento, Visitor) round
out the GoF set and each shows up in at least one classic case study.

## How each pattern folder is structured

```
PatternName/
├── notes.md        # intent, structure diagram, when to use / not use, interview variations
└── PatternName.cs  # runnable-shape C# example (+ .ts for the highest-frequency patterns)
```
