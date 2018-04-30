using AmudhaApp.Library.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    [JsonObject(Title = "product")]
    public class Product
    {
        [JsonProperty(PropertyName = "id"), JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; }

        [JsonProperty(PropertyName = "defaultPrice")]
        public ProductPrice Price { get; set; }

        [JsonProperty(PropertyName = "hsn")]
        public String Hsn { get; set; }

        [JsonProperty(PropertyName = "nickname")]
        public String Nickname { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
