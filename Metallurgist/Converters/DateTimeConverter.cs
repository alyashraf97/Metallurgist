using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Metallurgist.Converters
{
    public class DateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // Parse the long value as a Unix timestamp and convert it to a DateTime
            return DateTimeOffset.FromUnixTimeMilliseconds(reader.GetInt64()).DateTime;
        }

        public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            // Convert the DateTime value to a Unix timestamp and write it as a long
            writer.WriteNumberValue(new DateTimeOffset(value).ToUnixTimeMilliseconds());
        }
    }
}
