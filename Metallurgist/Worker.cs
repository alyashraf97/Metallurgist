using Metallurgist.Interfaces;
using Metallurgist.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Metallurgist
{
    public class Worker : BackgroundService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;
        //private readonly ILogger<Worker> _logger;
        private readonly IMetalPriceService _metalPriceService;

        public Worker(IMetalPriceService metalPriceService, IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
            _metalPriceService = metalPriceService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                IAsyncEnumerable<IronPrice> ironPrices = _metalPriceService.GetMetalPrices<IronPrice>("iron");
                IAsyncEnumerable<CopperPrice> copperPrices = _metalPriceService.GetMetalPrices<CopperPrice>("copper");
                IAsyncEnumerable<AluminumPrice> aluminumPrices = _metalPriceService.GetMetalPrices<AluminumPrice>("aluminum");

                await _metalPriceService.StoreMetalPricesInDatabase(ironPrices);
                await _metalPriceService.StoreMetalPricesInDatabase(copperPrices);
                await _metalPriceService.StoreMetalPricesInDatabase(aluminumPrices);

                await Task.Delay(TimeSpan.FromHours(1), stoppingToken);
            }
        }
    }
}