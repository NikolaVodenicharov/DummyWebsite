using System;
using System.Collections.Generic;
using System.Text;

namespace Tyres.Data.Models.Orders
{
    public class Cart : BaseCart
    {
        public IEnumerable<CartItem> Items { get; set; } = new List<CartItem>();
    }
}
