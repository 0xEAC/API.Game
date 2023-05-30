using Learning.eventArgs;
using Learning.Repositories;

namespace Learning.Models
{
  public class TickerService : AnimalsRepository
  {

    // Creates event
    public event EventHandler<TickerEventArgs>? Ticked;

    public TickerService()
    {
      // Subscribes functions to event + passes argument data
      Ticked += (o, args) => OnEvery10Seconds(args.Time);
      Ticked += (o, args) => OnEveryMinute(args.Time);
    }

    public void OnTick(TimeOnly time)
    {
      // Called on every tick
      Ticked?.Invoke(this, new TickerEventArgs(time));
    }
  }
}
