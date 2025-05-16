using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkBotLogic.Utilities
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class JSONDateTimeConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            switch (reader.TokenType)
            {
                case JsonTokenType.Null:
                    return null;
                case JsonTokenType.Number:
                    // Handle Unix timestamp (seconds)
                    long unixTime = reader.GetInt64();
                    return DateTimeOffset.FromUnixTimeSeconds(unixTime).DateTime;
                case JsonTokenType.String:
                    // Handle ISO 8601 string (fallback)
                    string dateString = reader.GetString();
                    if (DateTime.TryParse(dateString, out DateTime date))
                        return date;
                    return null;
                default:
                    throw new JsonException($"Unexpected token type: {reader.TokenType}");
            }
        }

        public override void Write(
            Utf8JsonWriter writer,
            DateTime? value,
            JsonSerializerOptions options)
        {
            if (value == null)
                writer.WriteNullValue();
            else
                // Write back as Unix timestamp (optional: use "o" for ISO 8601)
                writer.WriteNumberValue(new DateTimeOffset(value.Value).ToUnixTimeSeconds());
        }
    }
}
