namespace Metallurgist.Interfaces
{
    public interface IMetalPrice
    {
        public decimal Price { get; set; }
        public long Timestamp { get; set; }
    }
}
