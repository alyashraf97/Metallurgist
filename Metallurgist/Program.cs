using Metallurgist.Data;
using Metallurgist.Interfaces;
using Metallurgist.Services;

namespace Metallurgist
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.AddHttpClient();
                    services.AddSingleton<MetalPriceDbContextBase, MetalPriceDbContext>();
                    services.AddSingleton<IMetalPriceService, MetalPriceService>();
                    services.AddHostedService<Worker>();
                });
    }
}