# Observer

**Category**: Behavioral
**Intent**: Define a one-to-many dependency between objects so that when one
object (the **subject**) changes state, all its dependents (**observers**)
are notified automatically — without the subject knowing anything concrete
about them beyond the shared interface.

Extremely common in LLD interviews any time the prompt has "notify" in it:
seat-availability alerts, stock price tickers, event/notification systems,
YouTube channel subscriptions.

## Structure

```mermaid
classDiagram
    class Subject {
        <<interface>>
        +Subscribe(observer) void
        +Unsubscribe(observer) void
        +Notify() void
    }
    class StockTicker {
        -List~IObserver~ observers
        -decimal price
        +SetPrice(price) void
    }
    class IObserver {
        <<interface>>
        +Update(price) void
    }
    class MobileAppDisplay
    class EmailAlert

    Subject <|.. StockTicker
    IObserver <|.. MobileAppDisplay
    IObserver <|.. EmailAlert
    StockTicker o-- IObserver : notifies
```

`StockTicker` holds a list of `IObserver`s. When its price changes, it loops
over the list calling `Update(price)` on each — it never needs to know
whether an observer is a mobile display, an email alert, or something added
next month.

## Push vs Pull

- **Push model**: subject sends the changed data directly in `Update(data)`
  (shown above). Simple, but couples the observer interface to a specific
  payload shape.
- **Pull model**: subject just sends `Update(subjectRef)`; observers call
  back into the subject (`subject.GetPrice()`) to pull whatever data they
  need. More flexible when different observers need different subsets of
  state, at the cost of an extra call.

## When to use

- Multiple parts of the system need to react to a state change in one
  object, and that object shouldn't need to know the concrete types of
  everything reacting to it (decoupling publishers from subscribers).

## Observer vs Pub-Sub (a very common follow-up)

| | Observer (GoF) | Pub-Sub |
|---|---|---|
| Coupling | Subject holds direct references to observers | Publisher and subscriber don't know about each other at all — a broker/event bus sits between them |
| Typical scope | In-process, single application | Often distributed, across services (e.g. Kafka, SNS/SQS) |
| Delivery | Synchronous, in the notifying call | Often asynchronous, queued |

Interviewers sometimes ask you to note that Observer is the in-process
building block; a distributed notification system in a case study
(e.g. "design a notification service") usually escalates this to a
message-broker-based pub-sub, worth mentioning as the natural next step.

## Interview variations

- "Users should get notified the instant a parking spot frees up — how do
  you design that?" → Observer.
- "What if there are a huge number of observers, or notification should not
  block the caller?" → async dispatch / a queue between subject and
  observers — segues into pub-sub / message queues (HLD territory, good to
  flag the boundary).
- "Push or pull — which would you use here and why?"
