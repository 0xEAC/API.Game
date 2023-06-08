using API.Game.Backend.Models;

namespace API.Game.Backend.Ticker
{
    public class TickerBackgroundService : BackgroundService
    {

        // Include TickerService from Programs.cs -> AddSingleton
        private readonly TickerService _tickerService;

        public TickerBackgroundService(TickerService tickerService)
        {
            _tickerService = tickerService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // Executes OnTick every second
                _tickerService.OnTick(TimeOnly.FromDateTime(DateTime.Now));
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
