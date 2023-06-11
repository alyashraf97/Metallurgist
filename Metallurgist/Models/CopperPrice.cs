using Metallurgist.Interfaces;

namespace Metallurgist.Models
{
    public class CopperPrice : IMetalPrice
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}