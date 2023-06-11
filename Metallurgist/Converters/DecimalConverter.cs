using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Metallurgist.Converters
{
    public class DecimalConverter : JsonConverter<decimal>
    {
        // Make sure to have a public parameterless constructor
        public DecimalConverter()
        {
        }

        public override decimal Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Parse the string value as a decimal
            return decimal.Parse(reader.GetString());
        }

        public override void Write(Utf8JsonWriter writer, decimal value, JsonSerializerOptions options)
        {
            // Write the decimal value as a string
            writer.WriteStringValue(value.ToString());
        }
    }
}
