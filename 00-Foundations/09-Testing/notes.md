# Testing & Code Quality for LLD

Testing matters in LLD interviews for two reasons:

1. **Machine-coding rounds** (60-120 min, write working code) often expect
   at least a few tests, or ask "how would you test this?"
2. **Testability is a design signal.** Code that's hard to test is almost
   always badly coupled. "How would you unit-test this?" is really the
   interviewer asking "did you inject your dependencies?"

You don't need deep TDD theory. You need the vocabulary and the ability to
write a handful of meaningful tests quickly.

---

## Testability *is* design

The connection is direct and worth stating explicitly in an interview:

| Design problem | Testing symptom |
|---|---|
| Concrete dependency `new`'d internally (DIP violation) | Can't test without a real DB/network/clock |
| God class (SRP violation) | Every test needs elaborate setup for unrelated concerns |
| Static/Singleton state | Tests leak into each other; order-dependent failures |
| Logic in a `private` method you "need" to test | The method probably belongs in its own class |
| `DateTime.Now` used directly | Time-dependent behavior can't be tested deterministically |

> "If it's hard to test, it's usually badly designed" is a defensible
> one-liner, and it lets you connect testing back to
> [DIP](../03-SOLID-Principles/notes.md) rather than treating it as a
> separate topic.

The `DateTime.Now` case is worth internalizing — inject an `IClock`
abstraction instead, and parking-fee/late-fee logic becomes trivially
testable. This comes up in Parking Lot, Library Management, and Car Rental.

---

## AAA — the structure of every test

```csharp
[Fact]
public void Withdraw_WithInsufficientFunds_ReturnsFalse()
{
    // Arrange — set up the world
    var account = new BankAccount(100m);

    // Act — one action, the thing under test
    bool result = account.Withdraw(500m);

    // Assert — verify one outcome
    Assert.False(result);
    Assert.Equal(100m, account.GetBalance());
}
```

**Naming convention** that reads well in a test report and in an
interview: `MethodName_Scenario_ExpectedOutcome`.

---

## Test doubles — know the four names

Collectively called "test doubles"; interviewers do ask you to distinguish
them (especially stub vs mock).

| Double | What it does | Use when |
|---|---|---|
| **Dummy** | Passed to satisfy a signature, never actually used | A required parameter irrelevant to this test |
| **Stub** | Returns canned answers to calls | You need the dependency to *provide* something |
| **Fake** | A real, working, simplified implementation | `InMemoryRepository` instead of SQL |
| **Mock** | Records calls; the test asserts on *how it was called* | You need to verify an *interaction* happened |
| **Spy** | A real object that also records calls | You want real behavior plus call verification |

**Stub vs Mock — the distinction they're fishing for:**
- A **stub** helps you assert on **state** ("after this, the balance is 50").
- A **mock** helps you assert on **behavior** ("`SendEmail` was called
  exactly once with this address").

Prefer state-based assertions where you can; over-mocking produces tests
that break whenever you refactor internals, without catching real bugs.

---

## What to test in an LLD case study

For each case study, this checklist gives you meaningful coverage fast:

| Category | Example (Parking Lot) |
|---|---|
| **Happy path** | Park a car in an empty lot → ticket issued |
| **Invariant enforcement** | Assigning an occupied spot → throws |
| **Boundary** | Park in the last free spot; then the lot is full |
| **Invalid input** | Negative hours, null vehicle |
| **State transitions** | Legal ones succeed, illegal ones throw |
| **Polymorphic behavior** | Each vehicle type computes its own fee |
| **Extension point** | A new strategy works without modifying existing classes |

That last row is worth calling out: a test that **adds a new
implementation and asserts nothing else changed** is a *test of your OCP
claim*. It's an unusual and impressive thing to show.

---

## Example: testing the State pattern

State machines are the easiest thing to test convincingly — legal
transitions succeed, illegal ones throw:

```csharp
[Fact]
public void Order_CannotBeCancelled_OnceDelivered()
{
    var order = new Order();
    order.Pay();
    order.Ship();
    order.Deliver();

    Assert.Throws<InvalidOperationException>(() => order.Cancel());
}
```

See [`Tests/LLD.Foundations.Tests/`](../../Tests/LLD.Foundations.Tests/)
for runnable versions covering State, Strategy, Builder, and the
concurrency examples.

---

## Running the tests in this repo

```bash
dotnet build LLD-Claude.slnx     # compile everything
dotnet test  LLD-Claude.slnx     # run all tests
```

---

## Code quality signals interviewers actually notice

Beyond tests, in a machine-coding round:

- **Meaningful names** — `TryBook` over `Process`, `IsOccupied` over `flag`.
- **Small methods** that do one thing.
- **No magic numbers** — `const int MaxItems = 50;` not a bare `50`.
- **Fail fast** — validate at the boundary and throw with a clear message.
- **Consistent style** — pick a convention and hold it.
- **`readonly` / immutability** where the value shouldn't change after
  construction; it removes whole categories of bug.
- **Narrow return types** — `IReadOnlyList<T>` when callers shouldn't
  mutate (see [Domain Modeling](../07-Domain-Modeling/notes.md)).

---

## Interview variations

- "How would you test this class?" — if the answer requires a database,
  that's a DIP problem; say so and inject an interface.
- "What's the difference between a stub and a mock?" — state vs interaction.
- "How do you test time-dependent logic?" — inject an `IClock`.
- "How would you test that your design is extensible?" — add a new
  strategy/state in the test and assert existing behavior is unchanged.
- "How do you test concurrent code?" — run N parallel attempts and assert
  exactly one succeeds (see the concurrency tests); acknowledge that
  concurrency tests are probabilistic, not proofs.
