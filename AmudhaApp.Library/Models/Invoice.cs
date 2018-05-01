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
        public Guid Id { get; set; }

        [JsonProperty(PropertyName = "invoiceNumber")]
        public String InvoiceNumber { get; set; }

        [JsonProperty(PropertyName = "invoiceDate")]
        public String InvoiceDate { get; set; }

        [JsonProperty(PropertyName = "productList")]
        public List<KeyValuePair<Product,int>> ProductList { get; set; }

        [JsonProperty(PropertyName = "customer")]
        public Customer Customer { get; set; }

        [JsonProperty(PropertyName = "quantity")]
        public InventoryItem Quantity { get; set; }

        [JsonProperty(PropertyName = "otherCharges")]
        public double OtherCharges { get; set; }

        [JsonProperty(PropertyName = "totalAmount")]
        public double TotalAmount => ProductList.Sum(x => x.Key.Price.CalculatedPrice);

        [JsonProperty(PropertyName = "finalAmount")]
        public double FinalAmount => TotalAmount + OtherCharges;

        [JsonProperty(PropertyName = "subtotalAmount")]
        public double SubtotalAmount => ProductList.Sum(x => x.Key.Price.BasePrice);

        [JsonProperty(PropertyName = "updatedAt")]
        public DateTimeOffset UpdatedAt { get; set; }

    }
}
