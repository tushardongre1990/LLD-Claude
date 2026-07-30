// Condensed tour of all five SOLID principles in TypeScript.
// See notes.md and the C# files for the full violation/fix pairs — this
// file shows only the "fixed" (compliant) version of each, for brevity.

// ---------- S: Single Responsibility ----------
class FeeCalculator {
  calculate(hours: number): number {
    return 20 + 10 * hours;
  }
}

class ReceiptPrinter {
  print(ticketId: string, fee: number): void {
    console.log(`Receipt [${ticketId}]: ${fee}`);
  }
}

// ---------- O: Open/Closed ----------
abstract class Vehicle {
  abstract calculateFee(hours: number): number;
}

class Car extends Vehicle {
  calculateFee(hours: number): number {
    return 20 + 10 * hours;
  }
}

class Truck extends Vehicle {
  // Added later; Car/Vehicle untouched.
  calculateFee(hours: number): number {
    return 40 + 20 * hours;
  }
}

// ---------- L: Liskov Substitution ----------
interface Shape {
  area(): number;
}

class Rectangle implements Shape {
  constructor(private readonly width: number, private readonly height: number) {}
  area(): number {
    return this.width * this.height;
  }
}

class Square implements Shape {
  constructor(private readonly side: number) {}
  area(): number {
    return this.side * this.side;
  }
}

// ---------- I: Interface Segregation ----------
interface Workable {
  work(): void;
}

interface Feedable {
  eat(): void;
}

class RobotWorker implements Workable {
  work(): void {
    console.log("Robot working.");
  }
}

class HumanWorker implements Workable, Feedable {
  work(): void {
    console.log("Human working.");
  }
  eat(): void {
    console.log("Human eating.");
  }
}

// ---------- D: Dependency Inversion ----------
interface TicketRepository {
  save(ticketId: string): void;
}

class InMemoryRepository implements TicketRepository {
  private tickets: string[] = [];
  save(ticketId: string): void {
    this.tickets.push(ticketId);
  }
}

class ParkingLot {
  // Depends on the abstraction, injected via the constructor.
  constructor(private readonly repository: TicketRepository) {}

  issueTicket(ticketId: string): void {
    this.repository.save(ticketId);
  }
}

function demo(): void {
  const vehicles: Vehicle[] = [new Car(), new Truck()];
  vehicles.forEach((v) => console.log(v.calculateFee(2)));

  const shapes: Shape[] = [new Rectangle(5, 10), new Square(5)];
  shapes.forEach((s) => console.log(s.area()));

  const workers: Workable[] = [new HumanWorker(), new RobotWorker()];
  workers.forEach((w) => w.work());

  const lot = new ParkingLot(new InMemoryRepository());
  lot.issueTicket("T-1");
}

demo();
