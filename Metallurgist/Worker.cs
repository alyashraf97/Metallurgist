using Metallurgist.Interfaces;

namespace Metallurgist
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IMetalPriceService _metalPriceService;

        public Worker(ILogger<Worker> logger, IMetalPriceService metalPriceService)
        {
            _logger = logger;
            _metalPriceService = metalPriceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    var metals = new[] { "copper", "aluminum", "iron" };

                    foreach (var metal in metals)
                    {
                        var price = await _metalPriceService.GetMetalPrice(metal);
                        await _metalPriceService.UpdateLatestMetalPrice(metal, price);
                    }

                    _logger.LogInformation("Metal prices updated successfully.");

                    await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "An error occurred while updating metal prices.");
                }
            }
        }
    }
}