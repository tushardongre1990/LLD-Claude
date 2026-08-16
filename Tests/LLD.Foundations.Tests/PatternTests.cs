using Foundations.Patterns.Behavioral.State;
using Foundations.Patterns.Behavioral.Strategy;
using Foundations.Patterns.Creational.Builder;
using Foundations.Patterns.Creational.FactoryMethod;

namespace LLD.Foundations.Tests;

// Tests double as executable documentation: each one demonstrates the
// property the pattern is supposed to guarantee. See
// 00-Foundations/09-Testing/notes.md for the checklist these follow.

public class StatePatternTests
{
    [Fact]
    public void NewOrder_StartsInPlacedState()
    {
        var order = new Order();
        Assert.Equal("Placed", order.Status);
    }

    [Theory]
    [InlineData("Placed")]
    public void LegalTransitions_Succeed(string expectedStart)
    {
        var order = new Order();
        Assert.Equal(expectedStart, order.Status);

        order.Pay();
        Assert.Equal("Paid", order.Status);

        order.Ship();
        Assert.Equal("Shipped", order.Status);

        order.Deliver();
        Assert.Equal("Delivered", order.Status);
    }

    [Fact]
    public void Shipping_AnUnpaidOrder_IsRejected()
    {
        var order = new Order();
        Assert.Throws<InvalidOperationException>(() => order.Ship());
    }

    [Fact]
    public void Cancelling_AShippedOrder_IsRejected()
    {
        var order = new Order();
        order.Pay();
        order.Ship();

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    [Fact]
    public void Cancelling_ADeliveredOrder_IsRejected()
    {
        var order = new Order();
        order.Pay();
        order.Ship();
        order.Deliver();

        Assert.Throws<InvalidOperationException>(() => order.Cancel());
    }

    [Fact]
    public void Placed_CanBeCancelled_BeforePayment()
    {
        var order = new Order();
        order.Cancel();
        Assert.Equal("Cancelled", order.Status);
    }
}

public class StrategyPatternTests
{
    [Fact]
    public void HourlyStrategy_ChargesPerHour()
    {
        var ticket = new ParkingTicket(new HourlyFeeStrategy());
        Assert.Equal(30m, ticket.CalculateFee(3));
    }

    [Fact]
    public void FreeFirstHourStrategy_ChargesNothing_ForTheFirstHour()
    {
        var ticket = new ParkingTicket(new FreeFirstHourStrategy());
        Assert.Equal(0m, ticket.CalculateFee(1));
        Assert.Equal(20m, ticket.CalculateFee(3));
    }

    [Fact]
    public void FlatDayRate_IsIndependentOfHours()
    {
        var ticket = new ParkingTicket(new FlatDayRateStrategy());
        Assert.Equal(ticket.CalculateFee(1), ticket.CalculateFee(24));
    }

    // This is an OCP test: a brand-new strategy works with the EXISTING,
    // unmodified ParkingTicket class. That is the property Strategy is
    // supposed to buy you, asserted rather than merely claimed.
    private sealed class WeekendDoubleStrategy : IFeeStrategy
    {
        public decimal Calculate(int hours) => 20m * hours;
    }

    [Fact]
    public void NewStrategy_WorksWithoutModifyingExistingClasses()
    {
        var ticket = new ParkingTicket(new WeekendDoubleStrategy());
        Assert.Equal(40m, ticket.CalculateFee(2));
    }
}

public class BuilderPatternTests
{
    [Fact]
    public void Builder_ProducesConfiguredObject()
    {
        Pizza pizza = new PizzaBuilder()
            .WithSize("Large")
            .AddTopping("Mushroom")
            .WithExtraCheese()
            .Build();

        Assert.Equal("Large", pizza.Size);
        Assert.Single(pizza.Toppings);
        Assert.True(pizza.ExtraCheese);
    }

    // Regression test for a real bug: Build() must hand over a COPY of
    // the toppings list. If it passed the builder's own list, reusing the
    // builder afterwards would retroactively mutate an already-built,
    // supposedly-immutable Pizza.
    [Fact]
    public void BuiltPizza_IsNotMutated_WhenBuilderIsReusedAfterwards()
    {
        var builder = new PizzaBuilder().WithSize("Medium").AddTopping("Olives");
        Pizza first = builder.Build();

        builder.AddTopping("Jalapeno"); // mutating the builder, not the pizza

        Assert.Single(first.Toppings);
        Assert.Equal("Olives", first.Toppings[0]);
    }

    [Fact]
    public void Builder_AppliesDefaults_WhenNotSpecified()
    {
        Pizza pizza = new PizzaBuilder().Build();

        Assert.Equal("Medium", pizza.Size);
        Assert.Empty(pizza.Toppings);
        Assert.False(pizza.ExtraCheese);
    }
}

public class FactoryTests
{
    [Theory]
    [InlineData(VehicleType.Car, typeof(Car))]
    [InlineData(VehicleType.Motorcycle, typeof(Motorcycle))]
    [InlineData(VehicleType.Truck, typeof(Truck))]
    public void SimpleFactory_CreatesTheRequestedType(VehicleType type, Type expected)
    {
        Vehicle vehicle = SimpleVehicleFactory.Create(type);
        Assert.IsType(expected, vehicle);
    }

    [Fact]
    public void SimpleFactory_RejectsUnknownType()
    {
        Assert.Throws<ArgumentOutOfRangeException>(
            () => SimpleVehicleFactory.Create((VehicleType)999));
    }

    // Factory Method: the creator SUBCLASS determines the product, and
    // the shared workflow in the base class runs against it.
    [Fact]
    public void FactoryMethod_SubclassDeterminesProduct()
    {
        VehicleRegistration car = new CarRegistration();
        VehicleRegistration bike = new MotorcycleRegistration();

        Assert.Equal(20m + 10m * 2, car.SubmitToLot(2));
        Assert.Equal(10m + 5m * 2, bike.SubmitToLot(2));
    }
}

public class PolymorphismTests
{
    [Fact]
    public void EachVehicleType_ComputesItsOwnFee()
    {
        var vehicles = new List<Vehicle> { new Car(), new Motorcycle(), new Truck() };

        var fees = vehicles.Select(v => v.CalculateFee(2)).ToList();

        // No switch, no type checks — each subtype answered for itself.
        Assert.Equal(new[] { 40m, 20m, 80m }, fees);
    }
}
