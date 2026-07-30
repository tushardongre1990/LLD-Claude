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
│   ├── 01-OOP-Basics/
│   ├── 02-UML-Object-Oriented-Design/
│   ├── 03-SOLID-Principles/
│   ├── 04-Design-Patterns/
│   │   ├── Creational/
│   │   ├── Structural/
│   │   └── Behavioral/
│   └── 05-Interview-Approach/
└── 01-Case-Studies/        (filled in incrementally)
```

## Roadmap (based on "Grokking the LLD Interview" curriculum + more)

### Foundations — done in this pass
- [x] OOP Basics (encapsulation, abstraction, inheritance, polymorphism)
- [x] UML & Object-Oriented Design (class diagrams, relationships, use-case & sequence diagrams)
- [x] SOLID Principles
- [x] Design Patterns (Creational, Structural, Behavioral)
- [x] LLD Interview Approach (step-by-step framework, time budget, common mistakes)

### Case studies — added one at a time, on request

Every row assumes you already have OOP Basics + UML + SOLID (they underlie
every case study). The **Prerequisite patterns** column is what's
*additionally* needed — per the just-in-time workflow described in
[`01-Case-Studies/README.md`](01-Case-Studies/README.md), only read those
specific pattern notes before attempting that case study, not the whole
`04-Design-Patterns` folder. Patterns already covered in an earlier tier
aren't repeated in the "why" for later tiers, but they still apply.

Tiers are a **suggested difficulty/prerequisite order**, not a strict
requirement — jump to whichever case study you actually want.

#### Tier 1 — start here (1-2 new patterns each)
| # | Case Study | Prerequisite patterns | Status |
|---|---|---|---|
| 1 | Parking Lot System | Strategy (fee rules), Factory Method (vehicle creation), Singleton (the lot itself) | [ ] |
| 2 | Vending Machine | State (idle/selecting/dispensing), Strategy (payment methods) | [ ] |
| 3 | ATM System | State (ATM/session states), Chain of Responsibility (cash denomination dispensing), Command | [ ] |

#### Tier 2 — adds Observer (notify-driven systems)
| # | Case Study | Prerequisite patterns | Status |
|---|---|---|---|
| 4 | Elevator System | State (elevator states), Strategy (scheduling algorithm), Observer (floor requests) | [ ] |
| 5 | Library Management System | Strategy (fine/search rules), Observer (due-date reminders), Factory Method | [ ] |
| 6 | Amazon Locker Service | State (locker status), Strategy (size assignment), Observer (pickup notifications) | [ ] |
| 7 | Meeting Scheduler | Observer (invite notifications), Strategy (conflict resolution) | [ ] |
| 8 | Movie Ticket Booking System (BookMyShow-style) | State (seat/booking state), Strategy (pricing), Observer, **concurrency** (seat-locking race condition) | [ ] |
| 9 | Online Stock Brokerage System | Observer (price ticker — direct match to the Observer notes example), State (order state), Strategy (order types) | [ ] |

#### Tier 3 — adds Decorator / Command / Memento (richer behavior composition)
| # | Case Study | Prerequisite patterns | Status |
|---|---|---|---|
| 10 | Car Rental System | Strategy (pricing), State (reservation status), Decorator (add-ons/insurance), Factory Method | [ ] |
| 11 | Hotel Management System | Strategy (room pricing), State (booking status), Observer, Factory Method | [ ] |
| 12 | Restaurant Management System | State (order/table state), Strategy (billing), Observer | [ ] |
| 13 | Airline Management System | Strategy (pricing), State (flight/booking status), Observer, Factory Method | [ ] |
| 14 | Online Blackjack Game | State (game state), Strategy (game rules), Singleton (dealer/deck), Observer | [ ] |
| 15 | Chess Game | State (game state), Strategy (per-piece move validation via polymorphism), Command (move history), Memento (undo) | [ ] |

#### Tier 4 — adds Composite / Mediator (many-to-many, tree-shaped systems)
| # | Case Study | Prerequisite patterns | Status |
|---|---|---|---|
| 16 | Amazon Online Shopping System | Strategy (pricing/discounts), State (order state), Observer, Decorator, Facade (checkout) | [ ] |
| 17 | Stack Overflow | Composite (comments/answers tree), Observer (notifications), Strategy (ranking) | [ ] |
| 18 | Facebook (social network) | Observer (feed/notifications), Composite (comment threads), Mediator | [ ] |
| 19 | ESPNcricinfo | Observer (live score updates), State (match state), Strategy (scoring rules) | [ ] |
| 20 | LinkedIn | Observer (notifications/feed), Composite, Mediator | [ ] |
| 21 | Jigsaw Puzzle | Strategy (solving/fit algorithm), mostly core OOP | [ ] |
| 22 | Splitwise (bonus — very common ask, not in the source course) | Strategy (equal/percentage/exact split), Observer (notifications) | [ ] |
| 23 | Cab Booking / Ride Sharing (bonus — very common ask, not in the source course) | Strategy (fare calculation), State (ride state), Observer (location updates), Mediator (rider-driver matching) | [ ] |

Each case study folder will follow the same template:
`notes.md` (requirements → actors → use cases → class diagram → design pattern
choices → extensibility/edge cases → common interviewer follow-up variations)
plus `csharp/` and, for the highest-frequency ones, `typescript/` implementations.

See [`01-Case-Studies/README.md`](01-Case-Studies/README.md) for how each
session actually runs (just-in-time pattern coverage + interviewer-style
quizzing after you finish reading/implementing something).

## Suggested reading order

1. `00-Foundations/01-OOP-Basics`
2. `00-Foundations/02-UML-Object-Oriented-Design`
3. `00-Foundations/03-SOLID-Principles`
4. `00-Foundations/04-Design-Patterns` (skim the index first, then go pattern by pattern)
5. `00-Foundations/05-Interview-Approach`
6. Pick the first case study from the roadmap above.
# LLD-Claude
