namespace Metallurgist.Interfaces
{
    public interface IMetalPriceService
    {
        Task<decimal> GetMetalPrice(string metal);
        Task StoreMetalPricesInDatabase(string metal, IMetalPrice[] metalPrices);
        Task UpdateLatestMetalPrice(string metal, decimal price);
    }
}
