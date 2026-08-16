// Illustrates notes.md in this folder — the AbstractFactory pattern.
//   dotnet run --project Runner abstractfactory

namespace Foundations.Patterns.Creational.AbstractFactory;

public interface IEmailNotifier
{
    void SendEmail(string to, string body);
}

public interface ISmsNotifier
{
    void SendSms(string to, string body);
}

// --- US family ---
public class SesEmailNotifier : IEmailNotifier
{
    public void SendEmail(string to, string body) => Console.WriteLine($"[SES] to {to}: {body}");
}

public class TwilioSmsNotifier : ISmsNotifier
{
    public void SendSms(string to, string body) => Console.WriteLine($"[Twilio] to {to}: {body}");
}

// --- India family ---
public class SendgridEmailNotifier : IEmailNotifier
{
    public void SendEmail(string to, string body) => Console.WriteLine($"[Sendgrid] to {to}: {body}");
}

public class MsgClueSmsNotifier : ISmsNotifier
{
    public void SendSms(string to, string body) => Console.WriteLine($"[MsgClue] to {to}: {body}");
}

// The abstract factory: one interface, one creation method per product
// in the family.
public interface INotificationFactory
{
    IEmailNotifier CreateEmailNotifier();
    ISmsNotifier CreateSmsNotifier();
}

public class UsNotificationFactory : INotificationFactory
{
    public IEmailNotifier CreateEmailNotifier() => new SesEmailNotifier();
    public ISmsNotifier CreateSmsNotifier() => new TwilioSmsNotifier();
}

public class IndiaNotificationFactory : INotificationFactory
{
    public IEmailNotifier CreateEmailNotifier() => new SendgridEmailNotifier();
    public ISmsNotifier CreateSmsNotifier() => new MsgClueSmsNotifier();
}

// Client code depends only on the abstract factory + product interfaces —
// never on SesEmailNotifier / TwilioSmsNotifier directly.
public class NotificationService
{
    private readonly IEmailNotifier _email;
    private readonly ISmsNotifier _sms;

    public NotificationService(INotificationFactory factory)
    {
        _email = factory.CreateEmailNotifier();
        _sms = factory.CreateSmsNotifier();
    }

    public void NotifyUser(string contact, string message)
    {
        _email.SendEmail(contact, message);
        _sms.SendSms(contact, message);
    }
}

public static class AbstractFactoryDemo
{
    public static void Run()
    {
        var indiaService = new NotificationService(new IndiaNotificationFactory());
        indiaService.NotifyUser("user@example.com", "Your order shipped.");

        var usService = new NotificationService(new UsNotificationFactory());
        usService.NotifyUser("user@example.com", "Your order shipped.");
    }
}
