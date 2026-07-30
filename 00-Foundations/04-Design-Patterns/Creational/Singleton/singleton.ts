// TypeScript Singleton. Node.js is single-threaded (no data-race concern
// like C#), but the module system itself gives you a free, safe Singleton:
// a module's top-level state is evaluated once and cached by the module
// loader, so `export const instance = new ParkingLot()` at module scope is
// already a thread-safe-by-construction singleton — no locking pattern needed.
// The class-based version below is the portable/interview-friendly shape.

class ParkingLot {
  private static instance: ParkingLot | undefined;
  private activeTickets: string[] = [];

  // Private constructor: TypeScript enforces this only at compile time
  // (unlike C#'s runtime-enforced `private`), but it's still the idiomatic
  // way to signal "construct via getInstance() only."
  private constructor() {}

  static getInstance(): ParkingLot {
    if (!ParkingLot.instance) {
      ParkingLot.instance = new ParkingLot();
    }
    return ParkingLot.instance;
  }

  issueTicket(ticketId: string): void {
    this.activeTickets.push(ticketId);
  }

  get activeTicketCount(): number {
    return this.activeTickets.length;
  }
}

function demo(): void {
  const lot1 = ParkingLot.getInstance();
  const lot2 = ParkingLot.getInstance();
  console.log(lot1 === lot2); // true

  lot1.issueTicket("T-1");
  console.log(lot2.activeTicketCount); // 1
}

demo();
