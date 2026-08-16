namespace Foundations.Solid.Lsp.Violation
{
    // Square extends Rectangle: the textbook LSP violation. Setting Width on
    // a Square silently changes Height too, which breaks any caller that
    // treats it as a plain Rectangle.
    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }
        public int Area() => Width * Height;
    }

    public class Square : Rectangle
    {
        public override int Width
        {
            get => base.Width;
            set { base.Width = value; base.Height = value; }
        }

        public override int Height
        {
            get => base.Height;
            set { base.Width = value; base.Height = value; }
        }
    }

    public static class LspViolationDemo
    {
        public static void Run()
        {
            Rectangle r = new Square();
            r.Width = 5;
            r.Height = 10; // caller expects Width to stay 5; it silently became 10.
            Console.WriteLine(r.Area()); // 100, not the 50 a Rectangle caller would expect
        }
    }
}

namespace Foundations.Solid.Lsp.Fixed
{
    // Don't force a shared mutable-Width/Height contract on shapes that
    // don't actually share that behavior. Share only what's truly common.
    public interface IShape
    {
        int Area();
    }

    public class Rectangle : IShape
    {
        public int Width { get; }
        public int Height { get; }

        public Rectangle(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public int Area() => Width * Height;
    }

    public class Square : IShape
    {
        public int Side { get; }

        public Square(int side)
        {
            Side = side;
        }

        public int Area() => Side * Side;
    }

    public static class LspFixedDemo
    {
        public static void Run()
        {
            IShape[] shapes = { new Rectangle(5, 10), new Square(5) };
            foreach (var s in shapes)
                Console.WriteLine(s.Area()); // every IShape behaves exactly as its contract promises
        }
    }
}
