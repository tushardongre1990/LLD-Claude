// Illustrates notes.md in this folder — the Mediator pattern.
//   dotnet run --project Runner mediator

namespace Foundations.Patterns.Behavioral.Mediator;

public interface IChatMediator
{
    void AddUser(User user);
    void SendMessage(string message, User sender);
}

public abstract class User
{
    protected readonly IChatMediator Mediator;
    public string Name { get; }

    protected User(IChatMediator mediator, string name)
    {
        Mediator = mediator;
        Name = name;
    }

    public void Send(string message) => Mediator.SendMessage(message, this);

    public void Receive(string message) => Console.WriteLine($"{Name} received: {message}");
}

public class ChatUser : User
{
    public ChatUser(IChatMediator mediator, string name) : base(mediator, name) { }
}

// Concrete mediator: knows about all users, routes messages. Users only
// know the IChatMediator interface, never each other directly.
public class ChatRoom : IChatMediator
{
    private readonly List<User> _users = new();

    public void AddUser(User user) => _users.Add(user);

    public void SendMessage(string message, User sender)
    {
        foreach (var user in _users)
        {
            if (user != sender)
                user.Receive($"[{sender.Name}]: {message}");
        }
    }
}

public static class MediatorDemo
{
    public static void Run()
    {
        var room = new ChatRoom();

        var alice = new ChatUser(room, "Alice");
        var bob = new ChatUser(room, "Bob");
        var carol = new ChatUser(room, "Carol");

        room.AddUser(alice);
        room.AddUser(bob);
        room.AddUser(carol);

        alice.Send("Hey everyone!"); // Bob and Carol receive it; Alice does not echo to herself
    }
}
