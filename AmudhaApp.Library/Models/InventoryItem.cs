using System;
using System.Collections.Generic;
using System.Text;

namespace AmudhaApp.Library.Models
{
    public class InventoryItem : ProductsListItem
    {
        public Guid Id { get; set; }
        public InventoryItem(Product product) : base()
        {
            Product = product;
            Id = Product.Id;
        }
    }
}
