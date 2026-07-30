namespace Foundations.OopBasics;

// Encapsulation: state is private, all mutation goes through methods that
// enforce invariants. Balance can never go negative from outside this class.
public class BankAccount
{
    private decimal _balance;

    public BankAccount(decimal openingBalance)
    {
        if (openingBalance < 0)
            throw new ArgumentException("Opening balance cannot be negative.");

        _balance = openingBalance;
    }

    public decimal GetBalance() => _balance;

    public void Deposit(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Deposit amount must be positive.");

        _balance += amount;
    }

    public bool Withdraw(decimal amount)
    {
        if (amount <= 0)
            throw new ArgumentException("Withdrawal amount must be positive.");

        if (amount > _balance)
            return false; // insufficient funds — invariant enforced here, not by the caller

        _balance -= amount;
        return true;
    }
}

public static class EncapsulationDemo
{
    public static void Run()
    {
        var account = new BankAccount(100m);
        account.Deposit(50m);
        bool withdrew = account.Withdraw(500m); // false — protected by the class itself
        Console.WriteLine($"Balance: {account.GetBalance()}, last withdrawal succeeded: {withdrew}");
    }
}
