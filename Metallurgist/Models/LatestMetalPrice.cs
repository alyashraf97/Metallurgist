namespace Metallurgist.Models
{
    public class LatestMetalPrice
    {
        public int Id { get; set; }
        public string Metal { get; set; }
        public decimal Price { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
