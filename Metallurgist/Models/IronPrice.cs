using Metallurgist.Interfaces;
using System.ComponentModel;
using System.Text.Json.Serialization;

namespace Metallurgist.Models
{
    public class IronPrice : IMetalPrice
    {        
        public int Id { get; set; }

        //[JsonPropertyName("price")]
        //[JsonConverter(typeof(Converters.DecimalConverter))]
        public decimal Price { get; set; }

        //[JsonPropertyName("timestamp")]
        public long Timestamp { get; set; }
    }
}
