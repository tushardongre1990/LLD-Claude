// TypeScript often reaches for a plain object with optional properties
// instead of a Builder class (`function makePizza(opts: { size?: string, ... })`),
// since destructured optional params already solve "telescoping constructor."
// The class-based Builder still earns its keep when construction has *steps*
// with validation between them, or must stay fluent/chainable — shown here.

class Pizza {
  constructor(
    readonly size: string,
    readonly toppings: ReadonlyArray<string>,
    readonly extraCheese: boolean,
  ) {}

  toString(): string {
    return `${this.size} pizza with [${this.toppings.join(", ")}]${this.extraCheese ? " + extra cheese" : ""}`;
  }
}

class PizzaBuilder {
  private size = "Medium";
  private toppings: string[] = [];
  private extraCheese = false;

  withSize(size: string): this {
    this.size = size;
    return this;
  }

  addTopping(topping: string): this {
    this.toppings.push(topping);
    return this;
  }

  withExtraCheese(): this {
    this.extraCheese = true;
    return this;
  }

  build(): Pizza {
    return new Pizza(this.size, this.toppings, this.extraCheese);
  }
}

function demo(): void {
  const pizza = new PizzaBuilder()
    .withSize("Large")
    .addTopping("Mushroom")
    .addTopping("Olives")
    .withExtraCheese()
    .build();

  console.log(pizza.toString());
}

demo();
