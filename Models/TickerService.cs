using API.Game.Backend.eventArgs;
using API.Game.Backend.Repositories;

namespace API.Game.Backend.Models
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
