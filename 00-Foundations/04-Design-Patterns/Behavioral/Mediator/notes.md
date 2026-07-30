# Mediator

**Category**: Behavioral
**Intent**: Define an object that centralizes how a set of objects
("colleagues") interact, so those objects don't reference each other
directly — replacing many-to-many chatter (N² connections) with one-to-many
through a single mediator.

## The problem: N² wiring

Without a mediator, a chat room with 5 users where everyone can message
everyone directly means each `User` object holds references to every other
`User` — adding a 6th user means updating 5 existing objects. This is the
classic "tightly coupled peer web" problem.

## Structure

```mermaid
classDiagram
    class ChatMediator {
        <<interface>>
        +SendMessage(message, sender) void
        +AddUser(user) void
    }
    class ChatRoom
    ChatMediator <|.. ChatRoom

    class User {
        <<abstract>>
        #ChatMediator mediator
        +Send(message) void
        +Receive(message) void
    }
    class Alice
    class Bob
    User <|-- Alice
    User <|-- Bob
    User --> ChatMediator : only knows this
    ChatRoom o-- User : knows all users
```

Every `User` only knows about the `ChatMediator` interface, never about
other `User`s directly. `ChatRoom` (the concrete mediator) knows about all
users and handles routing messages between them. Adding a 6th user means
registering it with the mediator — zero changes to existing `User` objects.

## When to use

- Many objects need to communicate but you want to avoid a tangled web of
  direct references between all of them — centralize the interaction logic
  in one place.
- Real examples: chat rooms, air traffic control (planes don't talk to
  each other directly, they talk to the tower), UI dialogs where many
  widgets need to react to each other's changes (a `FormMediator`
  coordinating field validation/enabling).

## Mediator vs Facade — restated from Facade's notes

Facade is one-directional simplification for an external caller into a
subsystem; the subsystem classes don't know the facade exists. Mediator is
about the **peers themselves** routing their communication through a
central point — the colleagues are aware of and designed around the
mediator.

## Mediator vs Observer

They can combine (a mediator often uses Observer internally to notify
colleagues), but the *intent* differs: Observer is a **one-to-many
broadcast** from a single subject; Mediator is about **coordinating
many-to-many** interactions among peers, centralizing what would otherwise
be direct references between them.

## Interview variations

- "Design a chat room where users can message each other without every
  `User` holding references to every other `User`." → Mediator, by name.
- "How do you add a new user without modifying existing ones?" → register
  with the mediator only; existing `User` objects are untouched.
- "What's the risk of overusing Mediator?" → the mediator itself can become
  a god object accumulating too much logic — worth flagging as a trade-off
  if the interviewer pushes on it.
