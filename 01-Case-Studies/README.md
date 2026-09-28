# Case Studies

Machine-coding / LLD interview problems, **solved by you**, one at a time.
The order, and what to read before each, is in
[`../STUDY-PLAN.md`](../STUDY-PLAN.md).

There are no reference solutions here. Each folder is your own attempt,
created when you start that problem.

## How each session runs

- **Just-in-time concept coverage.** Before each case study, read only its
  "Read before" list in the study plan. `00-Foundations` stays as a standing
  reference for everything else.
- **Start from the prompt, not a spec.** Ask for the one-line prompt
  ("give me the Parking Lot prompt"). You get what an interviewer gives you —
  no requirements list. Extracting requirements through clarifying questions
  is part of what's being tested, and they're answered in character.
- **Design it yourself, timed.** Write your design into the folder's
  `notes.md` using the template below, within the timing targets. Then get it
  reviewed interviewer-style: which requirement, invariant or relationship
  you failed to extract, and which pattern you forced in or should have
  rejected — that's the actual signal.
- **Implement, then get the code reviewed.** C# plus tests, then a
  requirement change to absorb.
- **Active recall, not passive reading.** Expect follow-up questions that
  drill into your answers. Treat it as a mock interview.
- **Record what you missed.** The review goes in the folder's `review.md`.
  Fold the gaps into your own `notes.md`, and add one line per miss to
  [`MISTAKES.md`](MISTAKES.md) — re-reading that log before an interview is
  worth more than re-reading solutions.

## Timing targets

| Stage | Target |
|---|---|
| First attempt at a new case study | 30-40 min, notes allowed |
| Second pass / a similar problem | 20-30 min |
| Interview simulation | 45 min, **no notes**, narrate aloud throughout |

## Folder template

```
NN-CaseStudyName/     NN = the roadmap number, not the order you do it in
├── notes.md          your design, following the structure below
├── review.md         the interviewer-style review of it, and the follow-ups asked
└── csharp/           your implementation (tests live under Tests/)
```

## `notes.md` structure

Each case study's notes follow this order, which mirrors the framework in
[`../00-Foundations/05-Interview-Approach/notes.md`](../00-Foundations/05-Interview-Approach/notes.md):

1. **Problem statement** — the one-line prompt as an interviewer would give it
2. **Clarifying questions** — what to ask before designing, and why each matters
3. **Requirements** — functional / non-functional / explicitly out of scope / assumptions
4. **Actors & use cases**
5. **Core domain objects** — entities vs value objects
6. **Responsibilities** — which class owns what
7. **Invariants** — what must always be true (drives edge cases *and* concurrency)
8. **Class diagram** — mermaid, with relationship types called out
9. **Design walkthrough** — the reasoning, in the order you'd say it aloud
10. **Pattern selection** — for each: the problem it solves here, why it
    belongs, and **why the plausible alternatives were rejected**
11. **Sequence diagram** — for the one or two non-obvious flows
12. **State diagram** — where a lifecycle exists
13. **Concurrency** — where shared mutable state exists
14. **Implementation** — C#
15. **Tests** — happy path, invariants, boundaries, illegal transitions
16. **Edge cases**
17. **Extension exercises** — 5-10 "now add X" changes to attempt yourself
18. **Common interviewer follow-ups**
19. **Mistakes candidates make on this problem**

## Difficulty progression within a case study

Rather than one finished design, each case study builds in levels — this is
also how a real interview escalates:

| Level | Scope |
|---|---|
| **1 — Basic** | Minimal viable model; the happy path works |
| **2 — Extensible** | Multiple types/rules; the variation points are properly abstracted |
| **3 — Production concerns** | Concurrency, failure handling, invalid states |
| **4 — Follow-ups** | The interviewer's "now also support…" curveballs |

## Interview readiness checklist

A case study is *done* when you can do all of this without notes:

- [ ] Restate the requirements and assumptions in 2 minutes
- [ ] Name the actors and use cases
- [ ] Identify entities vs value objects
- [ ] State the key invariants
- [ ] Draw the class diagram and justify each relationship type
- [ ] Justify **every** interface — including why you *didn't* add others
- [ ] Explain each pattern choice and the alternative you rejected
- [ ] Code the core flow
- [ ] Identify the shared mutable state and how you'd protect it
- [ ] Answer 5 follow-up questions
- [ ] Absorb a new requirement without redesigning from scratch
