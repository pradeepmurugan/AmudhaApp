using AmudhaApp.Library.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    public class InventoryItem : ProductsListItem
    {
        [JsonProperty(PropertyName = "id"), JsonConverter(typeof(GuidConverter))]
        public Guid Id { get; set; }

        public InventoryItem(Product product) : base()
        {
            Product = product;
            Id = Product.Id;
        }
    }
}
