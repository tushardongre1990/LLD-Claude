# Case Studies

Machine-coding / LLD interview problems, added one at a time — see the
roadmap and order in the root [README.md](../README.md).

Just say which one you want next (e.g. "let's do Parking Lot") and it'll be
added here as its own folder.

## How each session runs

- **Just-in-time concept coverage.** Instead of reading all 23 design
  patterns up front, each case study pulls in only the concepts and
  patterns it actually needs (see its row in the roadmap table), covers
  those, and then solves the problem with them. `00-Foundations` stays as a
  standing reference for everything else.
- **Design it yourself first.** Read only the *requirements* section, then
  time yourself producing a class diagram and pattern choices before
  looking at the rest. Comparing afterwards tells you which requirement or
  relationship you failed to extract — that's the actual signal.
- **Active recall, not passive reading.** After you say you've read a case
  study's notes or finished implementing it, expect interview-style
  questions on it, with follow-ups that drill into your answers. Treat it
  as a mock interview.
- **Notes stay live.** Anything that comes out of that Q&A — an edge case,
  a variation, a clarification — gets folded back into that case study's
  `notes.md`, so the notes reflect the full discussion.

## Timing targets

| Stage | Target |
|---|---|
| First attempt at a new case study | 30-40 min, notes allowed |
| Second pass / a similar problem | 20-30 min |
| Interview simulation | 45 min, **no notes**, narrate aloud throughout |

## Folder template

```
NN-CaseStudyName/
├── notes.md
├── csharp/           full implementation
└── typescript/       only for the highest-frequency problems
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
14. **Implementation** — C# (+ TypeScript for the high-frequency ones)
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
