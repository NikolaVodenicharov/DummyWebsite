using System;
using System.Collections.Generic;
using System.Text;

namespace Tyres.Data.Models.Orders
{
    public class OrderItem : BaseItem
    {
        public Order Order { get; set; }
        public string OrderId { get; set; }
    }
}
