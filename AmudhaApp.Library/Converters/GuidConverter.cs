using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AmudhaApp.Library.Converters
{
    /// <summary>
    /// Default JSON.NET Guid Convert implementatation used Guid.toString("D"). This converter uses Guid.toString("N"), which saves a few chars.
    /// </summary>
    public class GuidConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(Guid);
        }
        public override bool CanRead
        {
            get
            {
                return false;
            }
        }
        public override bool CanWrite
        {
            get
            {
                return true;
            }
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var guid = (Guid)value;
            writer.WriteValue(guid.ToString("N"));
        }
        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
