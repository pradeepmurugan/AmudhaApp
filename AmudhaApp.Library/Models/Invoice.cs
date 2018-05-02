using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Newtonsoft.Json;
using AmudhaApp.Library.Converters;

namespace AmudhaApp.Library.Models
{
    [JsonObject(Title = "invoice")]
    public class Invoice
    {

        [JsonProperty(PropertyName = "id"), JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; } = Guid.Empty;

        [JsonProperty(PropertyName = "number")]
        public long Number { get; set; } = 0;

        [JsonProperty(PropertyName = "date")]
        public DateTimeOffset Date { get; set; }

        [JsonProperty(PropertyName = "productList")]
        public List<ProductsListItem> ProductsList { get; set; } = new List<ProductsListItem>();

        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; } = new Customer();
        
        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; } 


        public double TotalAmount() => ProductsList.Sum(x => x.Product.Price.CalculatedPrice * x.Quantity);

        public double SubtotalAmount() => ProductsList.Sum(x => x.Product.Price.BasePrice * x.Quantity);
    }
}
