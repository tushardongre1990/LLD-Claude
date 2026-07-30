interface FeeStrategy {
  calculate(hours: number): number;
}

class HourlyFeeStrategy implements FeeStrategy {
  calculate(hours: number): number {
    return 10 * hours;
  }
}

class FlatDayRateStrategy implements FeeStrategy {
  calculate(_hours: number): number {
    return 50;
  }
}

class FreeFirstHourStrategy implements FeeStrategy {
  calculate(hours: number): number {
    return hours <= 1 ? 0 : 10 * (hours - 1);
  }
}

class ParkingTicket {
  // TS idiom: strategies are often passed as plain functions instead of
  // classes implementing an interface, when there's no extra state to
  // carry. Both are shown here — pick whichever fits the case study.
  constructor(private readonly strategy: FeeStrategy) {}

  calculateFee(hours: number): number {
    return this.strategy.calculate(hours);
  }
}

// Function-based Strategy — equally valid in TS, less ceremony when the
// "strategy" has no internal state of its own.
type FeeStrategyFn = (hours: number) => number;

function calculateWithStrategy(hours: number, strategy: FeeStrategyFn): number {
  return strategy(hours);
}

function demo(): void {
  const promoTicket = new ParkingTicket(new FreeFirstHourStrategy());
  console.log(promoTicket.calculateFee(3)); // 20

  console.log(calculateWithStrategy(3, (h) => 10 * h)); // 30, function-based strategy
}

demo();
