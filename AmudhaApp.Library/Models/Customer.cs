using AmudhaApp.Library.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    [JsonObject(Title = "customer")]
    public class Customer
    {
        public Customer()
        {

        }

        [JsonProperty(PropertyName = "id"), JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonProperty(PropertyName = "name")]
        public String Name { get; set; } = "";

        [JsonProperty(PropertyName = "address")]
        public String Address { get; set; } = "";

        [JsonProperty(PropertyName = "contactNumber")]
        public String ContactNumber { get; set; } = "";

        [JsonProperty(PropertyName = "gstin")]
        public String GSTIN { get; set; } = "";

        [JsonProperty(PropertyName = "nickname")]
        public String Nickname { get; set; } = "";

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

        [JsonProperty(PropertyName = "createdAt")]
        public DateTimeOffset CreatedAt { get; set; }

        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }
    }
}
