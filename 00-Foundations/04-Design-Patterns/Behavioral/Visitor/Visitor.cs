namespace Foundations.Patterns.Behavioral.Visitor;

public interface IShapeVisitor
{
    void VisitCircle(Circle circle);
    void VisitSquare(Square square);
}

public abstract class Shape
{
    // Double dispatch: each concrete shape calls the visitor method
    // matching ITS OWN type, resolving the correct overload without any
    // type-checking in the visitor itself.
    public abstract void Accept(IShapeVisitor visitor);
}

public class Circle : Shape
{
    public double Radius { get; }
    public Circle(double radius) => Radius = radius;
    public override void Accept(IShapeVisitor visitor) => visitor.VisitCircle(this);
}

public class Square : Shape
{
    public double Side { get; }
    public Square(double side) => Side = side;
    public override void Accept(IShapeVisitor visitor) => visitor.VisitSquare(this);
}

// New operation #1 — zero changes to Circle/Square.
public class AreaVisitor : IShapeVisitor
{
    public double TotalArea { get; private set; }
    public void VisitCircle(Circle circle) => TotalArea += Math.PI * circle.Radius * circle.Radius;
    public void VisitSquare(Square square) => TotalArea += square.Side * square.Side;
}

// New operation #2 — again, zero changes to Circle/Square.
public class SvgExportVisitor : IShapeVisitor
{
    public void VisitCircle(Circle circle) =>
        Console.WriteLine($"<circle r=\"{circle.Radius}\" />");
    public void VisitSquare(Square square) =>
        Console.WriteLine($"<rect width=\"{square.Side}\" height=\"{square.Side}\" />");
}

public static class VisitorDemo
{
    public static void Run()
    {
        var shapes = new List<Shape> { new Circle(2), new Square(3) };

        var areaVisitor = new AreaVisitor();
        foreach (var shape in shapes)
            shape.Accept(areaVisitor);
        Console.WriteLine($"Total area: {areaVisitor.TotalArea:F2}");

        var svgVisitor = new SvgExportVisitor();
        foreach (var shape in shapes)
            shape.Accept(svgVisitor);
    }
}
