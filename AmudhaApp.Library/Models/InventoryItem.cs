using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    [JsonObject(Title = "inventoryItem")]
    public class InventoryItem
    {
        [JsonProperty(PropertyName = "product")]
        public Product Product { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public int Quantity { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
