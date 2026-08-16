using Foundations.OopBasics;

namespace LLD.Foundations.Tests;

// Covers 00-Foundations/01-OOP-Basics. Each test states the property the
// pillar is supposed to buy you, not just that the code runs.
// Composition lives in OopCompositionTests.cs — separate file so the two
// OopBasics namespaces don't collide on the name `Car`.

public class EncapsulationTests
{
    [Fact]
    public void Withdrawing_MoreThanTheBalance_IsRefused_AndLeavesBalanceUntouched()
    {
        var account = new BankAccount(100m);

        bool succeeded = account.Withdraw(500m);

        Assert.False(succeeded);
        Assert.Equal(100m, account.GetBalance());
    }

    [Fact]
    public void OpeningBalance_CannotBeNegative()
    {
        // The invariant is enforced at construction, so a BankAccount never
        // exists in an invalid state — that is the point of encapsulation.
        Assert.Throws<ArgumentException>(() => new BankAccount(-1m));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void Deposits_MustBePositive(decimal amount)
    {
        var account = new BankAccount(100m);

        Assert.Throws<ArgumentException>(() => account.Deposit(amount));
    }
}

public class RuntimeDispatchTests
{
    [Fact]
    public void FeeCalculation_DispatchesOnTheRuntimeType_NotTheDeclaredType()
    {
        // Both are declared as Vehicle; each runs its own override.
        Vehicle car = new Car("KA-01-1234");
        Vehicle bike = new Motorcycle("KA-01-5678");

        Assert.Equal(50m, car.CalculateParkingFee(3));   // 20 + 10 * 3
        Assert.Equal(25m, bike.CalculateParkingFee(3));  // 10 +  5 * 3
    }

    [Fact]
    public void AListOfBaseTypes_NeedsNoTypeSwitchToPriceCorrectly()
    {
        var vehicles = new List<Vehicle> { new Car("A"), new Motorcycle("B"), new Car("C") };

        decimal total = vehicles.Sum(v => v.CalculateParkingFee(2));

        Assert.Equal(100m, total); // 40 + 20 + 40 — no if/else on type anywhere
    }
}
