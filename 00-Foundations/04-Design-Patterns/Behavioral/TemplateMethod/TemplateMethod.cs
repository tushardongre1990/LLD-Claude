namespace Foundations.Patterns.Behavioral.TemplateMethod;

public record Record(string RawLine);

public abstract class DataImporter
{
    // The template: fixed sequence, sealed against reordering (not
    // virtual), lives exactly once.
    public void Import()
    {
        var raw = ReadSource();
        var records = Parse(raw);
        var valid = Validate(records);
        Save(valid);
    }

    protected abstract List<string> ReadSource();
    protected abstract List<Record> Parse(List<string> raw);

    // A step with a sensible default that subclasses MAY override
    // ("hook") but don't have to.
    protected virtual List<Record> Validate(List<Record> records) =>
        records.Where(r => !string.IsNullOrWhiteSpace(r.RawLine)).ToList();

    protected void Save(List<Record> records) =>
        Console.WriteLine($"Saved {records.Count} records.");
}

public class CsvImporter : DataImporter
{
    protected override List<string> ReadSource() =>
        new() { "id,name", "1,Alice", "2,Bob" };

    protected override List<Record> Parse(List<string> raw) =>
        raw.Skip(1).Select(line => new Record(line)).ToList(); // skip CSV header
}

public class JsonImporter : DataImporter
{
    protected override List<string> ReadSource() =>
        new() { "{\"id\":1}", "{\"id\":2}", "{\"id\":3}" };

    protected override List<Record> Parse(List<string> raw) =>
        raw.Select(line => new Record(line)).ToList();
}

public static class TemplateMethodDemo
{
    public static void Run()
    {
        DataImporter csv = new CsvImporter();
        csv.Import(); // Saved 2 records.

        DataImporter json = new JsonImporter();
        json.Import(); // Saved 3 records.
    }
}
