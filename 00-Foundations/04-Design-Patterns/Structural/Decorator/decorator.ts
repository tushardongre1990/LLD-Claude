abstract class Beverage {
  abstract cost(): number;
  abstract description(): string;
}

class Espresso extends Beverage {
  cost(): number {
    return 2.0;
  }
  description(): string {
    return "Espresso";
  }
}

abstract class BeverageDecorator extends Beverage {
  constructor(protected readonly inner: Beverage) {
    super();
  }
}

class MilkDecorator extends BeverageDecorator {
  cost(): number {
    return this.inner.cost() + 0.5;
  }
  description(): string {
    return this.inner.description() + " + Milk";
  }
}

class SugarDecorator extends BeverageDecorator {
  cost(): number {
    return this.inner.cost() + 0.25;
  }
  description(): string {
    return this.inner.description() + " + Sugar";
  }
}

function demo(): void {
  const order: Beverage = new SugarDecorator(new MilkDecorator(new Espresso()));
  console.log(`${order.description()} = $${order.cost().toFixed(2)}`);
  // Espresso + Milk + Sugar = $2.75
}

demo();
