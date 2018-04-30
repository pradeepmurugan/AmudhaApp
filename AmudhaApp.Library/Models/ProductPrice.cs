using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    [JsonObject(Title = "productPrice")]
    public class ProductPrice
    {
        [JsonProperty(PropertyName = "gstRate")]
        public Double GstRate { get; set; }

        [JsonProperty(PropertyName = "discountRate")]
        public Double DiscountRate { get; set; }

        [JsonProperty(PropertyName = "calculatedPrice")]
        public Double CalculatedPrice { get; set; }

        [JsonProperty(PropertyName = "basePrice")]
        public Double BasePrice { get; set; }

        [JsonProperty(PropertyName = "purchasePrice")]
        public Double PurchasePrice { get; set; }

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
