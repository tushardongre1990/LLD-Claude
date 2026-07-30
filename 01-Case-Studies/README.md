# Case Studies

Machine-coding / LLD interview problems, added one at a time — see the
roadmap and order in the root [README.md](../README.md#roadmap-based-on-grokking-the-lld-interview-curriculum--more).

Just say which one you want next (e.g. "let's do Parking Lot" or "do
Elevator System next") and it'll be added here as its own folder.

## Template every case study follows

```
NN-CaseStudyName/
├── notes.md
│   ├── Requirements (functional + explicitly out-of-scope)
│   ├── Actors & use cases
│   ├── Class diagram (mermaid) with relationships called out
│   ├── Design pattern choices — and WHY each one is used here
│   ├── Concurrency / edge cases
│   └── Common interviewer follow-up variations
├── csharp/
│   └── ... full implementation
└── typescript/          (only for the highest-frequency problems)
    └── ... full implementation
```

Apply the framework from
[`../00-Foundations/05-Interview-Approach/notes.md`](../00-Foundations/05-Interview-Approach/notes.md)
to each one: try to design it yourself first (10-15 min, timed), *then*
compare against the notes.

## How each session actually runs

- **Just-in-time pattern coverage.** Instead of reading all 23 design
  patterns up front, each case study links only the 2-4 patterns it
  actually needs (from the cheat sheet in
  [`../00-Foundations/04-Design-Patterns/README.md`](../00-Foundations/04-Design-Patterns/README.md)),
  covers those, and then solves the problem with them. The full pattern
  library in `00-Foundations` stays there as a standing reference for
  anything a case study doesn't need.
- **Active recall, not passive reading.** After you say you've read a
  case study's notes or finished implementing it, expect interview-style
  questions on it — including follow-ups that drill into your answers,
  the way a real interviewer would. Treat it as a mock interview, not a
  comprehension check.
- **Notes stay live.** Anything that comes out of that Q&A — an edge case
  you hadn't considered, a variation, a clarification — gets folded back
  into the case study's `notes.md` afterward, so the notes reflect the
  full discussion, not just the first draft.
