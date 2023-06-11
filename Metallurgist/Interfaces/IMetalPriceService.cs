namespace Metallurgist.Interfaces
{
    public interface IMetalPriceService
    {
        Task<List<IMetalPrice>> GetMetalPrices(string metal);
        Task StoreMetalPricesInDatabase(string metal, List<IMetalPrice> metalPrices);
        Task UpdateLatestMetalPrice(string metal, decimal price);
    }
}
