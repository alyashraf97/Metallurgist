namespace Metallurgist.Interfaces
{
    public interface IMetalPrice
    {
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
