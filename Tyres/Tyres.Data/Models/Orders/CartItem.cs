using System;
using System.Collections.Generic;
using System.Text;

namespace Tyres.Data.Models.Orders
{
    public class CartItem
    {
        public string ItemId { get; set; }

        public string CartId { get; set; }

        public int ProductId { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }

        public DateTime Date { get; set; }

    }
}
