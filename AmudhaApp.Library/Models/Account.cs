using AmudhaApp.Library.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    [JsonObject(Title = "account")]
    public class Account
    {
        public Account (Customer customer)
        {
            Customer = customer;
            Id = Customer.Id;
        }

        [JsonProperty(PropertyName = "id"), JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; } = new Customer();

        [JsonProperty(PropertyName = "details")]
        public string Details { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
        
    }
}
