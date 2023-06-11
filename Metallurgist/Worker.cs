using Metallurgist.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Metallurgist
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        private readonly ILogger<Worker> _logger;
        private readonly IMetalPriceService _metalPriceService;

        /*public Worker(ILogger<Worker> logger, IMetalPriceService metalPriceService)
        {
            _logger = logger;
            _metalPriceService = metalPriceService;
        }
        */

        public Worker(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // create a scope for the scoped service
                using (var scope = _serviceScopeFactory.CreateScope())
                {
                    // get the scoped service from the scope
                    var metalPriceService = scope.ServiceProvider.GetRequiredService<IMetalPriceService>();

                    // use the scoped service here
                    var metals = new[] { "copper", "aluminum", "iron" };

                    foreach (var metal in metals)
                    {
                        var prices = await metalPriceService.GetMetalPrices(metal);
                        await metalPriceService.StoreMetalPricesInDatabase(prices);
                        await metalPriceService.UpdateLatestMetalPrice(metal, prices);
                    }
                }

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}