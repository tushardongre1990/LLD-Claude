# Low Level Design (LLD) — Interview Prep Vault

This is your personal, self-paced LLD course. It is organized **topic-wise**: every
folder contains a `notes.md` (theory + diagrams) plus runnable code. Read the notes,
then read/run the code, then try to reproduce the class diagram and code from memory —
that active-recall loop is what actually sticks before an interview.

## Language strategy

- **C# is the primary language.** Every topic gets full C# code. C#'s explicit
  `interface`/`abstract class`/access modifiers/enums map almost 1:1 onto how
  interviewers talk about UML and OOP, so writing it in C# forces you to be precise
  about the exact thing interviewers are grading.
- **TypeScript is added for select topics** — enough that you can see the same
  pattern expressed in a structurally-typed, no-access-modifiers-enforced-at-runtime
  language, which is a common interview follow-up ("how would this differ in a
  duck-typed language?"). Not every file is duplicated in both languages.

## How this vault is built

Built **incrementally**: foundations first (this pass), then one machine-coding
case study at a time, added as you work through them. Just ask for the next one
(e.g. "let's do Parking Lot") when you're ready — see the roadmap below for the order.

## Folder map

```
LLD-Claude/
├── 00-Foundations/
│   ├── 01-OOP-Basics/                    ← the four pillars, composition vs inheritance
│   ├── 02-UML-Object-Oriented-Design/    ← class / sequence / use-case diagrams
│   ├── 03-SOLID-Principles/              ← + when SOLID becomes over-engineering
│   ├── 04-Design-Patterns/               ← all 23 GoF, + selection guide & comparisons
│   │   ├── Pattern-Selection-Guide.md    ← "I see this problem → consider this pattern"
│   │   ├── Pattern-Comparisons.md        ← every "X vs Y?" interview question
│   │   ├── Creational/ Structural/ Behavioral/
│   ├── 05-Interview-Approach/            ← the framework to run every case study through
│   ├── 06-Core-Design-Principles/        ← DRY, KISS, YAGNI, Tell-Don't-Ask, coupling/cohesion
│   ├── 07-Domain-Modeling/               ← entity vs value object, aggregates, invariants
│   ├── 08-Concurrency/                   ← race conditions, locking, optimistic vs pessimistic
│   └── 09-Testing/                       ← testability as a design signal, test doubles
├── 01-Case-Studies/                      (filled in incrementally)
├── Runner/                               ← run any demo: dotnet run --project Runner strategy
├── Tests/                                ← xUnit tests for the foundation code
└── LLD-Claude.slnx
```

Folders 01-05 are the **core path**. Folders 06-09 are **reference material
pulled in just-in-time** — you'll be pointed at them when a case study needs
them (concurrency shows up first in Movie Ticket Booking, domain modeling in
Splitwise, and so on).

## Running the code

```bash
dotnet build LLD-Claude.slnx          # compile everything
dotnet test  LLD-Claude.slnx          # run the test suite
dotnet run --project Runner           # list every runnable demo
dotnet run --project Runner strategy  # run one (state, concurrency, decorator, ...)
```

## Roadmap (based on "Grokking the LLD Interview" curriculum + more)

### Foundations — complete
- [x] OOP Basics (encapsulation, abstraction, inheritance, polymorphism)
- [x] UML & Object-Oriented Design (class diagrams, relationships, use-case & sequence diagrams)
- [x] SOLID Principles (+ where SOLID tips into over-engineering)
- [x] Design Patterns (all 23 GoF, + selection guide + comparison table)
- [x] LLD Interview Approach (step-by-step framework, time budget, common mistakes)
- [x] Core Design Principles (DRY, KISS, YAGNI, Tell-Don't-Ask, Law of Demeter, cohesion/coupling)
- [x] Domain Modeling (entity vs value object, aggregates, repositories, **invariants**)
- [x] Concurrency (race conditions, critical sections, optimistic vs pessimistic, deadlock)
- [x] Testing (testability as a design signal, test doubles, what to test in a case study)

### Case studies — added one at a time, on request

Every row assumes OOP Basics + UML + SOLID + Core Design Principles, which
underlie everything. The remaining columns are what that case study
*additionally* draws on — read only those before attempting it, per the
just-in-time workflow in
[`01-Case-Studies/README.md`](01-Case-Studies/README.md).

⚠️ **"Likely patterns" are candidates, not a shopping list.** A pattern
belongs in your design only if you can name the problem it solves *there*.
Several rows below are genuinely designable without the listed pattern —
Singleton in Parking Lot and Mediator in Cab Booking especially. Deciding
*not* to use one, and saying why, is a better answer than using it because
a table said so. See
[`00-Foundations/04-Design-Patterns/Pattern-Selection-Guide.md`](00-Foundations/04-Design-Patterns/Pattern-Selection-Guide.md).

Tiers are a suggested difficulty order, not a gate — jump wherever you want.

#### Tier 1 — start here
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 1 | Parking Lot System | composition, polymorphism, invariants | Strategy (fee rules), Factory | concurrency (two gates, one spot) | [ ] |
| 2 | Vending Machine | encapsulation, state transition table | State, Strategy (payment) | change-making algorithm | [ ] |
| 3 | ATM System | state, responsibility separation, invariants | State, Chain of Responsibility (denominations), Command | concurrency, transaction atomicity | [ ] |

#### Tier 2 — notification- and lifecycle-driven systems
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 4 | Elevator System | state modeling, scheduling | State, Strategy (dispatch algorithm) | concurrency, request queueing | [ ] |
| 5 | Library Management System | entities, value objects, repository | Strategy (fine rules), Observer (due dates) | date/time modeling | [ ] |
| 6 | Amazon Locker Service | state, size/fit assignment | State, Strategy, Observer | expiry timeouts | [ ] |
| 7 | Meeting Scheduler | **interval overlap**, value objects | Strategy (conflict resolution), Observer | recurring events, time zones | [ ] |
| 8 | Movie Ticket Booking ⭐ | entities, invariants, state | State (seat), Strategy (pricing), Observer | **concurrency — the seat-locking problem**, hold-with-timeout | [ ] |
| 9 | Online Stock Brokerage | order lifecycle, value objects (Money) | Observer (price feed), State (order), Command | partial fills, matching | [ ] |

#### Tier 3 — richer composition and game/domain rules
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 10 | Car Rental System | entities, date ranges | Strategy (pricing), State, Decorator (add-ons) | availability search | [ ] |
| 11 | Hotel Management System | aggregates, date ranges | Strategy (room pricing), State, Observer | overbooking policy | [ ] |
| 12 | Restaurant Management System | domain modeling, aggregates | State (order/table), Strategy (billing) | concurrency (table assignment) | [ ] |
| 13 | Airline Management System | complex relationships, value objects | Strategy (pricing), State, Observer | seat maps, multi-leg itineraries | [ ] |
| 14 | Online Blackjack Game | polymorphism, game state | State, Strategy (rules), Singleton (deck) | shuffling, multi-player turns | [ ] |
| 15 | Chess Game ⭐ | **polymorphic move validation**, board composition, invariants (king not in check) | Command (history), Memento (undo), State (turn) | check/checkmate detection | [ ] |

#### Tier 4 — large object graphs, trees, many-to-many
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 16 | Amazon Online Shopping | aggregates, Money, order lifecycle | Strategy (discounts), State, Observer, Facade (checkout) | inventory concurrency, idempotency | [ ] |
| 17 | Stack Overflow | tree structures, reputation rules | Composite (comment threads), Observer, Strategy (ranking) | vote integrity | [ ] |
| 18 | Facebook | large object graph, privacy rules | Observer (feed), Composite | feed generation at scale (→ HLD) | [ ] |
| 19 | ESPNcricinfo | event-driven state, scoring rules | Observer (live updates), State, Strategy | event replay | [ ] |
| 20 | LinkedIn | graph relationships, notifications | Observer, Composite | connection-degree queries | [ ] |
| 21 | Jigsaw Puzzle | object modeling, fit algorithm | Strategy (solver) | edge-matching efficiency | [ ] |
| 22 | Splitwise ⭐ (bonus) | **value objects (Money), invariants, rounding**, graph of debts | Strategy (equal/exact/percent split) | debt simplification, currency | [ ] |
| 23 | Cab Booking / Ride Sharing (bonus) | state, matching, value objects (Location) | Strategy (fare), State (ride), Observer (location) | concurrency (driver double-assignment), geo-indexing | [ ] |

⭐ = highest interview frequency / best value per hour spent.

Each case study folder will follow the same template:
`notes.md` (requirements → actors → use cases → class diagram → design pattern
choices → extensibility/edge cases → common interviewer follow-up variations)
plus `csharp/` and, for the highest-frequency ones, `typescript/` implementations.

See [`01-Case-Studies/README.md`](01-Case-Studies/README.md) for how each
session actually runs (just-in-time pattern coverage + interviewer-style
quizzing after you finish reading/implementing something).

## Suggested reading order

**Don't read all 23 patterns before starting case studies** — that's passive
learning and it's the mistake this vault is structured to avoid.

1. `00-Foundations/01-OOP-Basics`
2. `00-Foundations/02-UML-Object-Oriented-Design` — prioritize class and
   sequence diagrams; use-case diagrams are lower value, and you don't need
   to memorize UML notation beyond communicating a design.
3. `00-Foundations/03-SOLID-Principles` — including the over-engineering section
4. `00-Foundations/06-Core-Design-Principles`
5. `00-Foundations/07-Domain-Modeling` — the invariants section especially
6. `00-Foundations/04-Design-Patterns/README.md` (the index and
   [Pattern-Selection-Guide](00-Foundations/04-Design-Patterns/Pattern-Selection-Guide.md)
   only — **not** every pattern)
7. `00-Foundations/05-Interview-Approach`
8. **Start Tier 1 case studies.** Read individual pattern notes as each case
   study calls for them.

Pull in `08-Concurrency` when you reach Movie Ticket Booking (or earlier if
an interviewer asks), and `09-Testing` when you start writing case-study
code you want to verify.
# LLD-Claude
