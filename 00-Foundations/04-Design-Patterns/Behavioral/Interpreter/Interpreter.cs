// Illustrates notes.md in this folder — the Interpreter pattern.
//   dotnet run --project Runner interpreter

namespace Foundations.Patterns.Behavioral.Interpreter;

// Context: variable bindings available while evaluating.
public class Context
{
    private readonly Dictionary<string, int> _variables = new();

    public Context Set(string name, int value)
    {
        _variables[name] = value;
        return this;
    }

    public int Get(string name) =>
        _variables.TryGetValue(name, out int v)
            ? v
            : throw new KeyNotFoundException($"Undefined variable '{name}'.");
}

public interface IExpression
{
    int Interpret(Context context);
}

// --- Terminal expressions (leaves) ---
public class NumberExpression : IExpression
{
    private readonly int _value;
    public NumberExpression(int value) => _value = value;
    public int Interpret(Context context) => _value;
}

public class VariableExpression : IExpression
{
    private readonly string _name;
    public VariableExpression(string name) => _name = name;
    public int Interpret(Context context) => context.Get(_name);
}

// --- Non-terminal expressions (compose child expressions) ---
public class AddExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public AddExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Context context) =>
        _left.Interpret(context) + _right.Interpret(context);
}

public class MultiplyExpression : IExpression
{
    private readonly IExpression _left;
    private readonly IExpression _right;

    public MultiplyExpression(IExpression left, IExpression right)
    {
        _left = left;
        _right = right;
    }

    public int Interpret(Context context) =>
        _left.Interpret(context) * _right.Interpret(context);
}

public static class InterpreterDemo
{
    public static void Run()
    {
        // Represents:  x + (2 * y)
        //
        // NOTE: the tree is built by hand here. Turning the STRING
        // "x + 2 * y" into this tree is PARSING, which the Interpreter
        // pattern does not address — a distinction worth knowing.
        IExpression expression = new AddExpression(
            new VariableExpression("x"),
            new MultiplyExpression(
                new NumberExpression(2),
                new VariableExpression("y")));

        var context = new Context().Set("x", 10).Set("y", 5);

        Console.WriteLine($"x + (2 * y) = {expression.Interpret(context)}"); // 20

        // Same tree, different context — no rebuild needed.
        var other = new Context().Set("x", 1).Set("y", 1);
        Console.WriteLine($"x + (2 * y) = {expression.Interpret(other)}");   // 3
    }
}
