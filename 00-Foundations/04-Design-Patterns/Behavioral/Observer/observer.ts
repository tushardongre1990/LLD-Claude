interface StockObserver {
  update(symbol: string, price: number): void;
}

class StockTicker {
  private observers: StockObserver[] = [];
  private price = 0;

  constructor(private readonly symbol: string) {}

  subscribe(observer: StockObserver): void {
    this.observers.push(observer);
  }

  unsubscribe(observer: StockObserver): void {
    this.observers = this.observers.filter((o) => o !== observer);
  }

  setPrice(price: number): void {
    this.price = price;
    this.observers.forEach((o) => o.update(this.symbol, this.price));
  }
}

class MobileAppDisplay implements StockObserver {
  update(symbol: string, price: number): void {
    console.log(`[Mobile App] ${symbol} is now ${price}`);
  }
}

class EmailAlert implements StockObserver {
  constructor(private readonly threshold: number) {}

  update(symbol: string, price: number): void {
    if (price >= this.threshold) {
      console.log(`[Email] Alert: ${symbol} crossed ${this.threshold}, now at ${price}`);
    }
  }
}

// Note: in real Node.js code, EventEmitter (from "events") already IS an
// Observer-pattern implementation — `emitter.on("price", cb)` /
// `emitter.emit("price", data)` is idiomatic and usually preferred over
// hand-rolling subscribe/notify, unless the interview specifically wants
// you to demonstrate the pattern from scratch.
function demo(): void {
  const ticker = new StockTicker("ACME");
  const mobile = new MobileAppDisplay();
  const alert = new EmailAlert(100);

  ticker.subscribe(mobile);
  ticker.subscribe(alert);

  ticker.setPrice(95);
  ticker.setPrice(105);
}

demo();
