using Metallurgist.Converters;
using Metallurgist.Interfaces;
using Metallurgist.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;

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

        //CAN BARD FUCKING DO IT?

        public async IAsyncEnumerable<T> GetMetalPrices<T>(string metal) where T : IMetalPrice, new()
        {
            var options = new JsonSerializerOptions();
            options.Converters.Add(new DecimalConverter());
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"https://api.metals.live/v1/spot/{metal}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var asciiData = await response.Content.ReadAsByteArrayAsync();

            var dataString = Encoding.ASCII.GetString(asciiData);
            var data = ParseMetalPrices<T>(dataString);

            foreach (var item in data)
            {
                yield return item;
            }
        }

        public List<T> ParseMetalPrices<T>(string input) where T : IMetalPrice, new()
        {
            var items = new List<T>();
            var matches = Regex.Matches(input, @"\{""price"":""([\d.]+)"",""timestamp"":(\d+)\}");

            foreach (Match match in matches)
            {
                var item = new T
                {
                    Price = decimal.Parse(match.Groups[1].Value),
                    Timestamp = long.Parse(match.Groups[2].Value)
                };
                items.Add(item);
            }

            return items;
        }

        /*
        public async IAsyncEnumerable<T> GetMetalPrices<T>(string metal) where T : IMetalPrice, new()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"https://api.metals.live/v1/spot/{metal}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            var metalPriceArray = JsonSerializer.Deserialize<MetalPrice[]>(json, options);

            foreach (var item in metalPriceArray)
            {
                var price = decimal.Parse(item.Price);
                var timestamp = long.Parse(item.Timestamp);

                var metalPrice = new T
                {
                    Price = price,
                    Timestamp = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime
                };

                yield return metalPrice;
            }
        }
        */
        /*
        public async IAsyncEnumerable<T> GetMetalPrices<T>(string metal) where T: IMetalPrice, new()
        {
            var httpClient = _httpClientFactory.CreateClient();
            var url = $"https://api.metals.live/v1/spot/{metal}";

            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();

            var json = await response.Content.ReadAsStringAsync();

            var data = JsonSerializer.Deserialize<dynamic[]>(json);

            // Loop through each object in the array
            foreach (var item in data)
            {
                // Get the price and timestamp values from the object
                var price = decimal.Parse(item["price"].ToString());
                var timestamp = long.Parse(item["timestamp"].ToString());

                // Convert the timestamp from milliseconds to DateTime
                var date = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).DateTime;

                // Create a new T instance and set its properties
                var metalPrice = new T();
                metalPrice.Price = price;
                metalPrice.Timestamp = date;

                // Yield return the instance
                yield return metalPrice;
            }
        }*/

        public async Task StoreMetalPricesInDatabase<T>(IAsyncEnumerable<T> metalPrices) where T : class, IMetalPrice
        {
            await foreach (var metalPrice in metalPrices)
            {
                _dbContext.Set<T>().Add(metalPrice);
            }
            await _dbContext.SaveChangesAsync();
        }        
    }    
}







//var data = JsonSerializer.Deserialize<IDictionary<decimal, long>>(json);

//decimal price = data.GetProperty("price").GetDecimal();
//return price;

// Deserialize the json string into an array of anonymous objects

//public async Task StoreMetalPricesInDatabase(string metal, IAsyncEnumerable<IMetalPrice> metalPrices)
//{
//    switch (metal)
//    {
//        case "copper":
//            await StorePricesInDatabase<CopperPrice>(metalPrices);
//            break;
//        case "aluminum":
//            await StorePricesInDatabase<AluminumPrice>(metalPrices);
//            break;
//        case "iron":
//            await StorePricesInDatabase<IronPrice>(metalPrices);
//            break;
//    }
//}
/*
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
        }*/

/*
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
   }*/