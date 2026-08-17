using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Hitbtc.HitBtcModel
{
    internal sealed class OrderBookParameterConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(OrderBookParamter);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue,
            JsonSerializer serializer)
        {
            var value = JToken.Load(reader);
            var pair = value as JArray;
            if (pair == null || pair.Count < 2)
                throw new JsonSerializationException("An API v3 order-book entry must contain price and size.");

            return new OrderBookParamter
            {
                Price = pair[0].Value<string>(),
                Size = pair[1].Value<string>()
            };
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var entry = (OrderBookParamter)value;
            writer.WriteStartArray();
            writer.WriteValue(entry.Price);
            writer.WriteValue(entry.Size);
            writer.WriteEndArray();
        }
    }
}
