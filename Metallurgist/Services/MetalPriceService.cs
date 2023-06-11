using Metallurgist.Interfaces;
using Metallurgist.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace Metallurgist.Services
{
    public class MetalPriceService : IMetalPriceService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private MetalPriceDbContextBase _dbContext;

        public MetalPriceService(IHttpClientFactory httpClientFactory, MetalPriceDbContextBase dbContext)
        {
            _httpClientFactory = httpClientFactory;
            _dbContext = dbContext;
        }

        public async Task<List<IMetalPrice>> GetMetalPrices(string metal)
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"https://api.metals.live/v1/spot/{metal}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();
            //var data = JsonSerializer.Deserialize<IDictionary<decimal, long>>(json);

            //decimal price = data.GetProperty("price").GetDecimal();
            //return price;
            var priceList = await ObjectifyJson(json);
            return priceList;
        }

        public async Task StoreMetalPricesInDatabase(string metal,List<IMetalPrice> metalPrices)
        {
            switch (metal)
            {
                case "copper":
                    await StorePricesInDatabase<CopperPrice>(metalPrices);
                    break;
                case "aluminum":
                    await StorePricesInDatabase<AluminumPrice>(metalPrices);
                    break;
                case "iron":
                    await StorePricesInDatabase<IronPrice>(metalPrices);
                    break;
            }
        }

        private async Task StorePricesInDatabase<T>(List<IMetalPrice> metalPrices) where T : class, IMetalPrice
        {
            var existingTimestamps = _dbContext.Set<T>().Select(p => p.Timestamp).ToList();

            var newPrices = metalPrices
                .Where(p => !existingTimestamps.Contains(p.Timestamp))
                .Select(p => MetalPriceFactory.Create<T>(p.Price, p.Timestamp))
                .ToList();

            _dbContext.Set<T>().AddRange(newPrices);
            await _dbContext.SaveChangesAsync();
        }

        public async Task UpdateLatestMetalPrice(string metal, decimal price)
        {
            var latestPrice = await _dbContext.LatestMetalPrices.FindAsync(metal);

            if (latestPrice == null)
            {
                latestPrice = new LatestMetalPrice
                {
                    Metal = metal,
                    Price = price,
                    Timestamp = DateTime.UtcNow
                };
                _dbContext.LatestMetalPrices.Add(latestPrice);
            }
            else
            {
                latestPrice.Price = price;
                latestPrice.Timestamp = DateTime.UtcNow;
                _dbContext.LatestMetalPrices.Update(latestPrice);
            }

            await _dbContext.SaveChangesAsync();
        }

        public async Task<List<IMetalPrice>> ObjectifyJson(string json)
        {

            // Create a list to store the results
            var metalPrices = new List<IMetalPrice>();

            // Deserialize the json string into an array of anonymous objects
            var data = JsonSerializer.Deserialize<dynamic[]>(json);

            // Loop through each object in the array
            foreach (var item in data)
            {
                // Get the price and timestamp values from the object
                var price = decimal.Parse(item.price.ToString());
                var timestamp = long.Parse(item.timestamp.ToString());

                // Convert the timestamp from milliseconds to DateTime
                var date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

                // Create a new IMetalPrice instance and set its properties
                var metalPrice = new IMetalPrice
                {
                    Price = price,
                    Timestamp = date
                };

                // Add the instance to the list
                metalPrices.Add(metalPrice);
            }

            // Return the list
            return metalPrices;
        }

    }

    public static class MetalPriceFactory
    {
        public static T Create<T>(decimal price, DateTime timestamp) where T : IMetalPrice
        {
            if (typeof(T) == typeof(CopperPrice))
            {
                return (T)(object)new CopperPrice { Price = price, Timestamp = timestamp };
            }
            else if (typeof(T) == typeof(AluminumPrice))
            {
                return (T)(object)new AluminumPrice { Price = price, Timestamp = timestamp };
            }
            else if (typeof(T) == typeof(IronPrice))
            {
                return (T)(object)new IronPrice { Price = price, Timestamp = timestamp };
            }
            else
            {
                throw new ArgumentException("Invalid metal price type");
            }
        }
    }

    
}