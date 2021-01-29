using System;
using System.Globalization;
using System.Text.Json;

namespace OpenCredentialPublisher.ClrLibrary.Converters.Json
{
    public class DateConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
    {
        private const string Format = "o";
        public override DateTime Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options) =>
                DateTime.Parse(reader.GetString(),
                     CultureInfo.InvariantCulture);

        public override void Write(
            Utf8JsonWriter writer,
            DateTime dateTimeValue,
            JsonSerializerOptions options) =>
                writer.WriteStringValue(dateTimeValue.Kind == DateTimeKind.Utc ? dateTimeValue.ToString(Format) : dateTimeValue.ToUniversalTime().ToString(Format));
    }
}
