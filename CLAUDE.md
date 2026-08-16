# CLAUDE.md — working agreement for this repo

This file is the **portable, committed copy of everything an assistant needs to
continue this project on any machine**: who the user is, what the project is,
how it's built, and the conventions that were established through conversation
rather than written into the code.

Claude Code loads this file automatically from the repo root, so cloning the
repo is enough — no local machine state required.

> Maintenance: when a working preference or a project fact changes, update this
> file in the same commit as the change it describes. Treat it as source, not as
> a stale README appendix.

---

## 1. The user

- Preparing for **LLD (Low Level Design) interviews**, starting 2026-07-31, with
  **no prior LLD background**. Targeting **C#/.NET roles**.
- Optimizing for **retention and realistic interview practice**, not reading
  coverage. Passive reading of finished solutions is explicitly not the goal.
- **Commits manually.** See §5 — never run `git commit` or `git push` unless
  explicitly told to.

## 2. The project

A self-paced LLD interview-prep notes vault: foundations plus case studies, all
in one .NET solution so every example compiles, runs, and is tested.

- **Repo**: git, branch `main`, pushed to GitHub. Original path on the user's
  machine: `D:\Tushar\Courses\System-Design\LLD-Claude`.
- **Curriculum**: modeled on Educative's *"Grokking the Low Level Design
  Interview Using OOD Principles"* — foundations + 21 case studies, plus 2
  bonus ones (Splitwise, Cab Booking) that are common interview asks outside
  that course. The full roadmap with checkboxes lives in the root
  [`README.md`](README.md) — that is the single source of truth for progress.
- **Build approach** (user's choice): foundations built in full up front; case
  studies added **one at a time, on request** — never batch-generated.

### Status

**Foundations: complete and FROZEN as of 2026-08-16.** Ten sections under
`00-Foundations/`: 01-OOP-Basics, 02-UML, 03-SOLID, 04-Design-Patterns (all 23
GoF + `Pattern-Selection-Guide.md` + `Pattern-Comparisons.md`),
05-Interview-Approach, 06-Core-Design-Principles, 07-Domain-Modeling,
08-Concurrency, 09-Testing, 10-Anti-Patterns.

**`01-Case-Studies/` contains only a README template — no case studies written
yet.** That is the live edge of the project.

Frozen means: four external review rounds were run, the last found only minor
wording issues (all fixed) and explicitly recommended stopping.

- Do **not** make further foundation edits unless a case study exposes a real gap.
- Do **not** add new foundation topics. A later review proposed Clean
  Architecture, CQRS, event sourcing, Specification pattern, distributed locks,
  and idempotency/retry/timeout as standalone sections, plus a C#-language
  section. **All declined deliberately** — every added page delays the practice
  that actually closes the gap. Push new concepts into the case study that needs
  them (idempotency/retry/compensation belong in Movie Booking and Cab Booking).

## 3. Language: C# only

**C# is the sole implementation language.** The user initially chose "C# primary,
TypeScript for select topics" (2026-07-31), then on 2026-08-16 decided to
**delete TypeScript entirely** — all 7 `.ts` files and both `typescript/`
directories were removed and every reference scrubbed from the notes.

**Why**: focus. Targeting C#/.NET roles with a lot to learn already; a second
language was noise competing for attention. C#'s explicit
interfaces/abstract classes/access modifiers/enums also map closely to how
interviewers discuss UML and OOP.

**Apply**: write C# only, for foundations and every case study. Do not add
TypeScript examples, do not create `typescript/` folders in case-study
templates, do not suggest TS variants unless explicitly asked.
Language-comparison asides in prose ("most modern languages have Iterator
built in") are fine as general knowledge — but all *code* is C#.

## 4. Infrastructure

Targets **net10.0**. Projects: `00-Foundations/LLD.Foundations.csproj`,
`Runner/LLD.Runner.csproj`, `Tests/LLD.Foundations.Tests/`.

```bash
dotnet build LLD-Claude.slnx           # compile everything
dotnet test  LLD-Claude.slnx           # 45 xUnit tests, all passing (verified 2026-08-17)
dotnet run --project Runner            # list all 35 runnable demos
dotnet run --project Runner strategy   # run one by name (state, concurrency, command, ...)
dotnet run --project Runner all        # run every demo in sequence
```

**Always re-run build + tests after touching C# files**, and check that relative
markdown links still resolve after moving or renaming notes.

`.claude/settings.local.json` is committed and pre-allows the `dotnet` commands
above, so a fresh clone won't prompt for them.

---

## 5. Working conventions

### 5.1 Always hand over a commit message — unprompted

The user reviews and commits themselves, but does not want to write the message
or reconstruct what changed across a long session.

**After any change to files in this repo, provide a ready-to-paste commit
message without being asked.**

- Run `git status --short` / `git diff --stat` first so the message reflects
  what *actually* changed, not what was attempted.
- Conventional-commit style (`docs:`, `fix:`, `feat:`, `refactor:`): short
  subject, then a body explaining the **why** before the bullet list of *what*.
- Cover **only the uncommitted delta** — earlier work may already be committed
  mid-session.
- Deliver it at the end of the turn's work as part of the normal report. Don't
  ask whether they want one.
- **Do not run `git commit` or `git push`** unless explicitly told to.

### 5.2 How case-study sessions are taught

Corrected by the user on 2026-07-31 after an initial wrong approach:

1. **Just-in-time topic coverage, not front-loaded.** Don't march through all
   design patterns first and only then solve a case study. For each case study,
   identify its 2–4 relevant patterns from
   [`00-Foundations/04-Design-Patterns/README.md`](00-Foundations/04-Design-Patterns/README.md),
   briefly link/summarize *just those*, then solve it. (The foundations stay as
   a complete lookup reference — this is about *sequencing*, not deletion.)
2. **Quiz like a real interviewer after self-reported completion.** When the
   user says they've read a topic or implemented something, don't just move on —
   ask interview-style questions, with follow-ups that drill deeper based on
   their answers. Active recall, not a rhetorical "any questions?".
3. **Feed gaps back into the notes.** Anything that surfaces during that Q&A and
   isn't already in the relevant `notes.md` — an edge case, a variation, a
   clarification — gets added afterward, so the vault stays complete even though
   it was written before the conversation happened.

**Why**: the user is optimizing for retention, wants material to appear only when
it's needed to solve something, and wants testing built into the loop.

### 5.3 Notes teach judgment, not absolutes

An external review (2026-08-16) flagged several notes for stating design rules
too absolutely. Fixed across the vault; this is the standing standard.

These framings are **wrong** and must not reappear:

| Wrong | Correct |
|---|---|
| "Any `switch(type)` violates OCP" | Only when the switch grows as the domain grows. A small switch over a genuinely fixed set is fine. |
| "`new` inside business logic is a red flag" | Only for **infrastructure** (DB, HTTP, clock, SDKs). `new` for value objects and owned domain objects is correct. |
| "Every design pattern exists to satisfy OCP/DIP/SRP" | Too broad. Several predate SOLID and exist for other reasons (Flyweight = memory, Iterator = traversal encapsulation). |
| Mapping a case study to a mandatory pattern list | Patterns are **candidates**. Some commonly-listed ones (Singleton in Parking Lot, Mediator in Cab Booking) are genuinely optional. |

**Why**: reflexive abstraction is a mid-level interview tell. "I could extract a
Strategy, but there's one rule in the requirements — I'd add it when a second
appears" scores higher than adding the interface automatically. Notes that teach
rules-as-absolutes train the wrong instinct.

**Apply**: pair each principle with its failure mode and its counterweight
(KISS/YAGNI), and for every pattern selection record **why the plausible
alternative was rejected**. The case-study `notes.md` template in
[`01-Case-Studies/README.md`](01-Case-Studies/README.md) has a dedicated slot
for this.

### 5.4 Concurrency examples must be executed, not just tested

Concurrency/invariant code has shipped **three** times with passing tests that
didn't exercise the broken path:

1. `VersionedSeat.TryBook` had a check-then-act window between the version CAS
   and the `_bookedBy` write. Unit test passed; running the demo showed **300
   winners instead of 1**.
2. `SeatBooking.TryBookAll` claimed "all-or-nothing", but the rollback branch
   only `Console.WriteLine`d "Rolling back X" without releasing anything. The
   test passed because it only ever failed on the *first* seat, so the rollback
   loop had nothing to iterate.
3. `ConcurrencyDemo` printed "Unsafe seat ended up owned by: X (many threads
   believed they had won)" — an assertion *in output text* that was false on
   ~97% of runs. Fixed 2026-08-17 to run 200 trials and report the true count.

**Empirical calibration (measured 2026-08-17 — don't re-derive):** the classic
`if (seat.IsAvailable) seat.Book(...)` race reproduces in only **~2–3% of trials
even at `Parallel.For(0, 1000)`** — the first thread wins within nanoseconds, so
the window is tiny. Raising contention barely helps (10 threads ≈ 1000 threads).
Two consequences:

- "Run the demo once and watch it break" is **wrong advice** — loop the whole
  experiment ~200× and count.
- **A winner that varies between runs is NOT evidence of a race** — that's just
  scheduling of the single winner. You must count how many threads passed the
  check.

**Why this matters here specifically**: concurrency bugs are probabilistic, so a
green test is weak evidence — and in a *teaching* repo a wrong example is worse
than no example, because it trains the wrong mental model on exactly the topic
the user is least equipped to check. Note failure mode (3): prose and comments
asserting runtime behaviour need verifying just like code does.

**Apply**, for any concurrency or invariant-enforcing example:

- Run the demo under real contention (`Parallel.For` with ~1000 iterations), not
  just the unit test.
- Write the test so it hits the failure/partial path specifically, not just the
  happy path.
- Verify each regression test is **non-vacuous** by temporarily reverting the fix
  and confirming the test fails.
- Check that comments claiming a property ("atomic", "all-or-nothing",
  "thread-safe") are actually true of the code beneath them.

The same discipline caught a Builder mutability bug.

---

## 6. Continuing the work

When picking up this project:

1. Read the roadmap in [`README.md`](README.md) and take the **next unchecked
   case study**, unless the user names one.
2. Follow the 19-section `notes.md` template in
   [`01-Case-Studies/README.md`](01-Case-Studies/README.md): requirements →
   invariants → class diagram → pattern selection *with rejected alternatives* →
   sequence/state diagrams → concurrency → code → tests → extension exercises →
   interviewer follow-ups.
3. Run the teaching loop from §5.2 — don't just hand over a finished solution.
4. Re-run `dotnet build` + `dotnet test`, then hand over a commit message (§5.1).

Tier 1 (Parking Lot → Vending Machine → ATM) is the suggested entry point;
⭐-marked studies (Movie Ticket Booking, Chess, Splitwise) are the highest
interview frequency per hour spent.

## 7. Repo layout

```
LLD-Claude/
├── CLAUDE.md                             ← this file
├── README.md                             ← reading order, run commands, roadmap
├── LLD-Claude.slnx
├── .claude/settings.local.json           ← committed; pre-allows dotnet commands
├── 00-Foundations/                       ← FROZEN; notes.md per topic + C# where useful
│   ├── 01-OOP-Basics/ … 10-Anti-Patterns/
│   └── 04-Design-Patterns/{Creational,Structural,Behavioral}/
├── 01-Case-Studies/                      ← README template only; filled in one at a time
├── Runner/                               ← `dotnet run --project Runner <demo>`
└── Tests/LLD.Foundations.Tests/          ← 45 xUnit tests
```

Folder numbers are **stable IDs, not a reading order** — the reading order is
the table in `README.md`. Conceptual topics (UML, core principles, domain
modeling, testing, anti-patterns) are notes-only by design.
