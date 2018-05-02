using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    [JsonObject(Title = "productsListItem")]
    public class ProductsListItem
    {
        public ProductsListItem()
        {

        }
        public ProductsListItem (Product product, long quantity)
        {
            Product = product;
            Quantity = quantity;
        }
        [JsonProperty(PropertyName = "product")]
        public Product Product { get; set; } = new Product();

        [JsonProperty(PropertyName = "quantity")]
        public long Quantity { get; set; } = 0;

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }
    }
}
