namespace Foundations.Solid.Isp;

namespace Violation
{
    // A fat interface forces every implementer to deal with methods that
    // may not apply to it.
    public interface IWorker
    {
        void Work();
        void Eat();
        void Sleep();
    }

    public class HumanWorker : IWorker
    {
        public void Work() => Console.WriteLine("Human working.");
        public void Eat() => Console.WriteLine("Human eating.");
        public void Sleep() => Console.WriteLine("Human sleeping.");
    }

    public class RobotWorker : IWorker
    {
        public void Work() => Console.WriteLine("Robot working.");

        // Forced to implement methods that don't make sense for a robot.
        public void Eat() => throw new NotSupportedException("Robots don't eat.");
        public void Sleep() => throw new NotSupportedException("Robots don't sleep.");
    }
}

namespace Fixed
{
    // Split by role. Implement only what applies.
    public interface IWorkable
    {
        void Work();
    }

    public interface IFeedable
    {
        void Eat();
    }

    public interface ISleepable
    {
        void Sleep();
    }

    public class HumanWorker : IWorkable, IFeedable, ISleepable
    {
        public void Work() => Console.WriteLine("Human working.");
        public void Eat() => Console.WriteLine("Human eating.");
        public void Sleep() => Console.WriteLine("Human sleeping.");
    }

    // No forced no-op / throwing methods.
    public class RobotWorker : IWorkable
    {
        public void Work() => Console.WriteLine("Robot working.");
    }

    public static class IspDemo
    {
        public static void Run()
        {
            IWorkable[] workers = { new HumanWorker(), new RobotWorker() };
            foreach (var w in workers)
                w.Work();
        }
    }
}
