namespace Zaggoware.Games.Common.Converters
{
    using System;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class JsonTimeSpanConverter : JsonConverter<TimeSpan>
    {
        public override TimeSpan Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            return TimeSpan.TryParse(reader.GetString(), out var timeSpan) ? timeSpan : TimeSpan.Zero;
        }

        public override void Write(Utf8JsonWriter writer, TimeSpan value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }
}