namespace Metallurgist.Interfaces
{
    public interface IMetalPriceService
    {
        IAsyncEnumerable<T> GetMetalPrices<T>(string metal) where T: IMetalPrice, new();
        Task StoreMetalPricesInDatabase<T>(IAsyncEnumerable<T> metalPrices) where T: class, IMetalPrice;
    }
}
