# Design Patterns — Index

Design patterns are named solutions to recurring design problems. In an LLD
interview they matter for two reasons:

1. **Vocabulary/speed** — saying "I'll use Strategy here" communicates an
   entire structure in three words instead of you re-deriving it live.
2. **They're worked examples of good design goals** — recurring ways to get
   low coupling, extensibility, clear separation of responsibilities, and
   encapsulation of what varies. SOLID (see
   [`../03-SOLID-Principles/notes.md`](../03-SOLID-Principles/notes.md))
   helps *explain* why many of these structures pay off, but a pattern is
   not simply "an implementation of a SOLID principle" — several predate
   SOLID and exist for reasons of their own (Flyweight is a memory
   optimization; Iterator is about traversal encapsulation).

**Anti-pattern to avoid in interviews**: don't force a pattern in just to
namedrop it. Interviewers notice "resume-driven design." Only reach for a
pattern once you've identified the actual problem it solves (varying
algorithm → Strategy; notifying multiple listeners → Observer; complex
multi-step construction → Builder; etc.). Every case study in
`01-Case-Studies` will call out which patterns are used and *why*, not just
that they're used.

## How each pattern page is laid out

Every pattern's `notes.md` is self-contained and follows the same shape:
diagram → the actual code inline → 📄 a link to the full file and its
`dotnet run --project Runner <name>` command → a **Try it** prompt. Read
straight through; open the `.cs` file only when you want to *change*
something. The names in the diagrams match the code exactly, so nothing
needs re-mapping as you read.

The **Try it** prompts are the highest-value part of each page. Most of them
ask you to *break* the pattern — delete the defensive copy, skip the redo
clear, add the element type that forces every visitor to change — because
feeling a trade-off is what lets you argue about it in an interview, and
"when would you *not* use this?" is the question that separates recall from
judgment.

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
| [Interpreter](Behavioral/Interpreter/notes.md) | Behavioral | Represent a grammar and evaluate sentences in it | Expression evaluator, simple rule DSL |

## The three families, one sentence each

- **Creational** — patterns about *how objects get made* (hide `new` behind
  something smarter).
- **Structural** — patterns about *how classes/objects are composed* into
  larger structures without their code becoming tangled.
- **Behavioral** — patterns about *how objects communicate and share
  responsibility* for a behavior/algorithm.

## Two companion pages you should use constantly

- **[Pattern-Selection-Guide.md](Pattern-Selection-Guide.md)** — "I see
  *this* problem in my design → consider *that* pattern." Use it while
  designing a case study.
- **[Pattern-Comparisons.md](Pattern-Comparisons.md)** — every
  "what's the difference between X and Y?" pair in one place. Use it while
  revising; these are among the most common interview questions.

## Not all 23 deserve equal mastery

Know all of them, but budget your effort by how often they actually decide
a case study:

| Tier | Patterns | Target depth |
|---|---|---|
| **A — know cold** | Strategy, Factory (Simple/Method), Observer, State, Decorator, Builder, Adapter, Facade, Command | Can design with it, code it from memory, and justify it against alternatives |
| **B — understand & implement** | Singleton, Abstract Factory, Composite, Proxy, Chain of Responsibility, Mediator, Memento, Template Method | Can explain the structure and write it with a moment's thought |
| **C — recognize & explain** | Prototype, Flyweight, Bridge, Iterator, Visitor, Interpreter | Can define it, name a use case, and recognize when someone else applies it |

Learn Tier A first — it covers the large majority of what actually comes up
in case studies. Under the just-in-time approach (see
[`../../01-Case-Studies/README.md`](../../01-Case-Studies/README.md)) you'll
pick these up as the case studies demand them rather than all at once.

## How each pattern folder is structured

```
PatternName/
├── notes.md        # intent, structure diagram, when to use / not use, interview variations
└── PatternName.cs  # runnable C# example (via `dotnet run --project Runner <name>`)
```
