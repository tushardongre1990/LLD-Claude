// All four OOP pillars in one file, TypeScript-flavored.
//
// TS-specific notes vs C#:
// - TypeScript's type system is *structural*, not nominal: a class satisfies
//   an interface just by having the right shape, no `implements` required
//   (though writing `implements` is still good practice for clarity + compiler
//   checks). C# is nominal: a class must explicitly declare `: IFoo`.
// - True private state needs `#field` (real runtime-private, ES2022) or a
//   closure; the `private` keyword is compile-time-only and erased at
//   runtime, so reflection/JS access can still reach a `private` field.
// - No multiple class inheritance, same as C#, but a class can `implements`
//   multiple interfaces just like C#.

// ---------- 1. Encapsulation ----------
class BankAccount {
  #balance: number; // real private field, not just a TS compile-time private

  constructor(openingBalance: number) {
    if (openingBalance < 0) throw new Error("Opening balance cannot be negative.");
    this.#balance = openingBalance;
  }

  getBalance(): number {
    return this.#balance;
  }

  deposit(amount: number): void {
    if (amount <= 0) throw new Error("Deposit amount must be positive.");
    this.#balance += amount;
  }

  withdraw(amount: number): boolean {
    if (amount <= 0) throw new Error("Withdrawal amount must be positive.");
    if (amount > this.#balance) return false;
    this.#balance -= amount;
    return true;
  }
}

// ---------- 2. Abstraction ----------
interface PaymentProcessor {
  pay(amount: number): boolean;
}

class UpiProcessor implements PaymentProcessor {
  pay(amount: number): boolean {
    console.log(`Charged ${amount} via UPI.`);
    return true;
  }
}

class Checkout {
  constructor(private readonly processor: PaymentProcessor) {}

  completeOrder(total: number): boolean {
    return this.processor.pay(total);
  }
}

// ---------- 3. Inheritance ----------
abstract class Vehicle {
  constructor(public readonly licensePlate: string) {}

  displayPlate(): void {
    console.log(`Plate: ${this.licensePlate}`);
  }

  abstract calculateParkingFee(hours: number): number;
}

class Car extends Vehicle {
  calculateParkingFee(hours: number): number {
    return 20 + 10 * hours;
  }
}

class Motorcycle extends Vehicle {
  calculateParkingFee(hours: number): number {
    return 10 + 5 * hours;
  }
}

// ---------- 4. Polymorphism ----------
function printFees(vehicles: Vehicle[]): void {
  for (const vehicle of vehicles) {
    // Same runtime dispatch idea as C#: no type-check/switch needed here.
    console.log(`${vehicle.licensePlate}: ${vehicle.calculateParkingFee(2)}`);
  }
}

function demo(): void {
  const account = new BankAccount(100);
  account.deposit(50);
  console.log("Balance:", account.getBalance(), "Withdraw 500 ok?", account.withdraw(500));

  const checkout = new Checkout(new UpiProcessor());
  checkout.completeOrder(499);

  const vehicles: Vehicle[] = [new Car("KA-01-1111"), new Motorcycle("KA-01-2222")];
  printFees(vehicles);
}

demo();
