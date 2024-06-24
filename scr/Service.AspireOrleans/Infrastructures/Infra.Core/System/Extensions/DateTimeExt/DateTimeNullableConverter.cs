using System.Text.Json;
using System.Text.Json.Serialization;

namespace Infra.Core.System.Extensions.DateTimeExt
{
    public class DateTimeNullableConverter : JsonConverter<DateTime?>
    {
        public override DateTime? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var result = default(DateTime?);
            if (DateTime.TryParse(reader.GetString(), out DateTime datetime))
            {
                result = datetime;
            }

            return result;
        }

        public override void Write(Utf8JsonWriter writer, DateTime? value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value?.ToString("yyyy-MM-dd HH:mm:ss"));
        }
    }
}
