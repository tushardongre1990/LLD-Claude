namespace Foundations.Patterns.Behavioral.ChainOfResponsibility;

public enum TicketSeverity { Low, Medium, Critical }
public record SupportTicket(string Description, TicketSeverity Severity);

public abstract class SupportHandler
{
    private SupportHandler? _next;

    public SupportHandler SetNext(SupportHandler next)
    {
        _next = next;
        return next; // lets callers chain SetNext calls fluently
    }

    public void Handle(SupportTicket ticket)
    {
        if (CanHandle(ticket))
        {
            Resolve(ticket);
        }
        else if (_next != null)
        {
            _next.Handle(ticket);
        }
        else
        {
            Console.WriteLine($"No handler could resolve: {ticket.Description}");
        }
    }

    protected abstract bool CanHandle(SupportTicket ticket);
    protected abstract void Resolve(SupportTicket ticket);
}

public class L1SupportHandler : SupportHandler
{
    protected override bool CanHandle(SupportTicket ticket) => ticket.Severity == TicketSeverity.Low;
    protected override void Resolve(SupportTicket ticket) => Console.WriteLine($"[L1] Resolved: {ticket.Description}");
}

public class L2SupportHandler : SupportHandler
{
    protected override bool CanHandle(SupportTicket ticket) => ticket.Severity == TicketSeverity.Medium;
    protected override void Resolve(SupportTicket ticket) => Console.WriteLine($"[L2] Resolved: {ticket.Description}");
}

public class L3SupportHandler : SupportHandler
{
    protected override bool CanHandle(SupportTicket ticket) => ticket.Severity == TicketSeverity.Critical;
    protected override void Resolve(SupportTicket ticket) => Console.WriteLine($"[L3] Resolved: {ticket.Description}");
}

public static class ChainOfResponsibilityDemo
{
    public static void Run()
    {
        var l1 = new L1SupportHandler();
        var l2 = new L2SupportHandler();
        var l3 = new L3SupportHandler();
        l1.SetNext(l2).SetNext(l3);

        l1.Handle(new SupportTicket("Password reset", TicketSeverity.Low));       // handled by L1
        l1.Handle(new SupportTicket("App crashing", TicketSeverity.Critical));    // forwarded to L3
    }
}
