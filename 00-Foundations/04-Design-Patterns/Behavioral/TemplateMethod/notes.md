# Template Method

**Category**: Behavioral
**Intent**: Define the **skeleton of an algorithm** in a base class method,
deferring specific steps to subclasses — the overall sequence of steps is
fixed and shared; only individual steps vary.

## Structure

```mermaid
classDiagram
    class DataImporter {
        <<abstract>>
        +Import() void
        #ReadSource()* List~string~
        #Parse(raw)* List~Record~
        #Validate(records) List~Record~
        #Save(records) void
    }
    class CsvImporter
    class JsonImporter
    DataImporter <|-- CsvImporter
    DataImporter <|-- JsonImporter
    note for DataImporter "Import() is NOT overridden —\nit calls the steps below in\na fixed order (the 'template')"
```

```csharp
// The template — fixed sequence, lives once in the base class, never
// duplicated or reordered by subclasses.
public void Import()
{
    var raw = ReadSource();
    var records = Parse(raw);
    var valid = Validate(records); // has a default implementation subclasses can reuse
    Save(valid);
}
```

`CsvImporter` and `JsonImporter` each override only `ReadSource()` and
`Parse()` — the overall pipeline (read → parse → validate → save) is
written exactly once, in the base class, and can't be accidentally
reordered or skipped by a subclass.

## When to use

- Multiple classes implement the **same overall algorithm/process** with
  only specific steps differing — you want the shared sequence in exactly
  one place (DRY), while still allowing per-subclass customization of
  individual steps.
- Common real examples: data import/export pipelines, game turn loops
  (`StartTurn → PlayTurn → EndTurn`, shared across game types), test
  framework lifecycle (`SetUp → RunTest → TearDown`), report generation.

## Template Method vs Strategy

| | Template Method | Strategy |
|---|---|---|
| Mechanism | **Inheritance** — subclasses override specific steps of a fixed base algorithm | **Composition** — a whole algorithm is swapped in as an object |
| Granularity | Varies individual **steps** within one algorithm | Varies the **entire algorithm** as one unit |
| Flexibility | Fixed at compile time (which subclass you instantiate) | Swappable at runtime (which strategy object you inject) |

If you need to swap behavior *at runtime*, prefer Strategy. If the overall
sequence is genuinely fixed and shared, and only specific steps legitimately
vary per subtype, Template Method is the more direct fit — and is a case
where inheritance (rather than composition) is actually the right call, per
the composition-vs-inheritance discussion in `01-OOP-Basics`.

## Interview variations

- "Multiple report types share the same generate → format → export flow,
  but the formatting step differs — how do you avoid duplicating the
  pipeline in every report class?" → Template Method.
- "Why not Strategy here?" → the *entire sequence* is shared and fixed;
  only isolated steps vary — inheritance-based step overriding is a more
  direct fit than swapping a whole algorithm object.
