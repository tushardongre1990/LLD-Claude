# Low Level Design (LLD) — Interview Prep Vault

A self-paced LLD course built for interview preparation, in **C#**. Foundations
are complete; case studies are added one at a time as you work through them.

---

## Start here

### 1. Read a little, solve a case study, repeat

Read [`01-OOP-Basics`](00-Foundations/01-OOP-Basics/notes.md),
[`02-UML`](00-Foundations/02-UML-Object-Oriented-Design/notes.md) and
[`03-SOLID`](00-Foundations/03-SOLID-Principles/notes.md) first. After that,
**[`STUDY-PLAN.md`](STUDY-PLAN.md) is the path**: for every case study it lists
exactly which topics and sections to read *before* attempting it. Topics are
read just before the first problem that needs them — never all 23 design
patterns up front, which is passive learning and exactly what this vault is
structured to avoid.

### 2. Then run this loop on every case study

**You solve the case studies yourself** — there are no reference solutions to
peek at.

```
Get the one-line prompt (ask "give me the X prompt")
        ↓
Ask clarifying questions — answered as an interviewer would
        ↓
Design it yourself — timed, in your own notes.md
  (entities → invariants → class diagram → what varies → patterns)
        ↓
Get it reviewed interviewer-style — what did you fail to extract?
        ↓
Implement in C# + write 3-8 meaningful tests, then get the code reviewed
        ↓
Handle the follow-ups and a requirement change
        ↓
Log what you missed in MISTAKES.md → next case study
```

**The design-it-yourself step is the whole point.** Reading a finished
solution feels productive and teaches very little; the interview value is in
discovering what you missed. Timing targets and the `notes.md` template are in
[`01-Case-Studies/README.md`](01-Case-Studies/README.md).

### 3. Two rules that keep this honest

- **Patterns are candidates, never a checklist.** A pattern belongs in your
  design only if you can name the problem it solves *there*. Deciding *not*
  to use one, and saying why, is a stronger interview answer than using it
  because a table listed it.
- **Notes stay live.** When quizzing surfaces something the notes don't
  cover — an edge case, a variation — it gets folded back in.

---

## Running the code

```bash
dotnet build LLD-Claude.slnx          # compile everything
dotnet test  LLD-Claude.slnx          # run the test suite (45 tests)
dotnet run --project Runner           # list every runnable demo
dotnet run --project Runner strategy  # run one (state, concurrency, command, ...)
```

## Language

**C# only.** Its explicit `interface`/`abstract class`/access modifiers/enums map
almost 1:1 onto how interviewers talk about UML and OOP, so writing it in C# forces
you to be precise about exactly what's being graded. One language, no translation
overhead, everything compiles and runs from one solution.

## Folder map

```
LLD-Claude/
├── 00-Foundations/
│   ├── 01-OOP-Basics/                    ← the four pillars, composition vs inheritance
│   ├── 02-UML-Object-Oriented-Design/    ← class / sequence / state / use-case diagrams
│   ├── 03-SOLID-Principles/              ← + when SOLID becomes over-engineering
│   ├── 04-Design-Patterns/               ← all 23 GoF, + selection guide & comparisons
│   │   ├── Pattern-Selection-Guide.md    ← "I see this problem → consider this pattern"
│   │   ├── Pattern-Comparisons.md        ← every "X vs Y?" interview question
│   │   └── Creational/ Structural/ Behavioral/
│   ├── 05-Interview-Approach/            ← the framework + refactoring-signals table
│   ├── 06-Core-Design-Principles/        ← DRY, KISS, YAGNI, Tell-Don't-Ask, coupling/cohesion
│   ├── 07-Domain-Modeling/               ← entity vs value object, invariants, Money, use cases
│   ├── 08-Concurrency/                   ← race conditions, locking, optimistic vs pessimistic
│   ├── 09-Testing/                       ← testability as a design signal, failure paths
│   └── 10-Anti-Patterns/                 ← recognizing bad designs (asked directly in interviews)
├── 01-Case-Studies/                      ← filled in incrementally, one at a time
├── Runner/                               ← run any demo by name
├── Tests/                                ← xUnit tests for the foundation code
└── LLD-Claude.slnx
```

Folder numbers are **stable IDs, not a reading order** — the order is
[`STUDY-PLAN.md`](STUDY-PLAN.md).

Every folder has a `notes.md`; most also carry runnable C# code. Conceptual
topics (UML, core principles, domain modeling, testing, anti-patterns) are
notes-only by design.

---

## Roadmap

### Foundations — complete and frozen
- [x] OOP Basics (encapsulation, abstraction, inheritance, polymorphism)
- [x] UML & Object-Oriented Design (class, sequence, state, use-case diagrams)
- [x] SOLID Principles (+ where SOLID tips into over-engineering)
- [x] Design Patterns (all 23 GoF + selection guide + comparison table)
- [x] LLD Interview Approach (framework, time budget, refactoring signals, common mistakes)
- [x] Core Design Principles (DRY, KISS, YAGNI, Tell-Don't-Ask, Law of Demeter, cohesion/coupling)
- [x] Domain Modeling (entity vs value object, aggregates, **invariants**, Money, application services)
- [x] Concurrency (race conditions, critical sections, optimistic vs pessimistic, deadlock)
- [x] Testing (testability as a design signal, test doubles, failure paths)
- [x] Anti-Patterns (god object, anemic model, primitive obsession, premature abstraction)

**Further theory now has diminishing returns.** The remaining gap is
practice — anything new gets learned inside the case study that needs it.

### Case studies — solved by you, one at a time

The `#` is each study's stable ID (and its folder number). The order to do
them in, and exactly what to read before each, is in
[`STUDY-PLAN.md`](STUDY-PLAN.md) — tick progress there.

⚠️ **"Likely patterns" are candidates, not a shopping list.** Several rows
are genuinely designable without the listed pattern. See the
[Pattern Selection Guide](00-Foundations/04-Design-Patterns/Pattern-Selection-Guide.md).

Tiers group problems by what they exercise; the plan interleaves them.

#### Tier 1 — start here
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 1 | Parking Lot System | composition, polymorphism, invariants | Strategy (fee rules), Factory | *(single-threaded first; concurrency — two gates, one spot — is a later extension)* | [ ] |
| 2 | Vending Machine | encapsulation, state transition table | State, Strategy (payment) | change-making algorithm | [ ] |
| 3 | ATM System | state, responsibility separation, invariants | State, Chain of Responsibility (denominations), Command | concurrency, transaction atomicity | [ ] |

#### Tier 2 — notification- and lifecycle-driven systems
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 4 | Elevator System | state modeling, scheduling | State, Strategy (dispatch algorithm) | concurrency, request queueing | [ ] |
| 5 | Library Management System | entities, value objects, repository | Strategy (fine rules), Observer (due dates) | date/time modeling | [ ] |
| 6 | Amazon Locker Service | state, size/fit assignment | State, Strategy, Observer | expiry timeouts | [ ] |
| 7 | Meeting Scheduler | **interval overlap** (`startA < endB && startB < endA`, half-open ranges), value objects | Strategy (conflict resolution), Observer | recurring events, time zones | [ ] |
| 8 | Movie Ticket Booking ⭐ | entities, invariants, state | State (seat), Strategy (pricing), Observer | **concurrency — the seat-locking problem**, hold-with-timeout | [ ] |
| 9 | Online Stock Brokerage | order lifecycle, value objects (Money) | Observer (price feed), State (order), Command | partial fills, matching | [ ] |

#### Tier 3 — richer composition and game/domain rules
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 10 | Car Rental System | entities, date ranges | Strategy (pricing), State, Decorator (add-ons) | availability search | [ ] |
| 11 | Hotel Management System | aggregates, date ranges | Strategy (room pricing), State, Observer | overbooking policy | [ ] |
| 12 | Restaurant Management System | domain modeling, aggregates | State (order/table), Strategy (billing) | concurrency (table assignment) | [ ] |
| 13 | Airline Management System | complex relationships, value objects | Strategy (pricing), State, Observer | seat maps, multi-leg itineraries | [ ] |
| 14 | Online Blackjack Game | polymorphism, game state | State, Strategy (rules), *maybe* Factory (card/deck creation) | shuffling, multi-player turns | [ ] |
| 15 | Chess Game ⭐ | **polymorphic move validation**, board composition, invariants (king not in check) | Command (history), Memento (undo), State (turn) | check/checkmate detection | [ ] |

#### Tier 4 — large object graphs, trees, many-to-many
| # | Case Study | Core concepts | Likely patterns | Advanced concerns | Status |
|---|---|---|---|---|---|
| 16 | Amazon Online Shopping | aggregates, Money, order lifecycle | Strategy (discounts), State, Observer, Facade (checkout) | inventory concurrency, idempotency | [ ] |
| 17 | Stack Overflow | tree structures, reputation rules | Composite (comment threads), Observer, Strategy (ranking) | vote integrity | [ ] |
| 18 | Facebook | large object graph, **graph modelling**, privacy rules | Observer (feed) | feed generation at scale (→ HLD) | [ ] |
| 19 | ESPNcricinfo | event-driven state, scoring rules | Observer (live updates), State, Strategy | event replay | [ ] |
| 20 | LinkedIn | **graph relationships** (User ↔ Connection), notifications | Observer | connection-degree queries | [ ] |
| 21 | Jigsaw Puzzle | object modeling, fit algorithm | Strategy (solver) | edge-matching efficiency | [ ] |
| 22 | Splitwise ⭐ (bonus) | **value objects (Money), invariants, rounding**, graph of debts | Strategy (equal/exact/percent split) | debt simplification, currency | [ ] |
| 23 | Cab Booking / Ride Sharing (bonus) | state, matching, value objects (Location) | Strategy (fare), State (ride), Observer (location) | concurrency (driver double-assignment), geo-indexing | [ ] |

⭐ = highest interview frequency / best value per hour spent.

Each case study folder follows the template in
[`01-Case-Studies/README.md`](01-Case-Studies/README.md): **your** `notes.md`
(requirements → invariants → class diagram → pattern choices *with rejected
alternatives* → sequence/state diagrams → concurrency → edge cases →
extension exercises → interviewer follow-ups), a `review.md` recording the
interviewer-style review of it, and your `csharp/` implementation.

*Curriculum based on "Grokking the LLD Interview" (21 case studies), plus
Splitwise and Cab Booking — both common interview asks not in that course.*
