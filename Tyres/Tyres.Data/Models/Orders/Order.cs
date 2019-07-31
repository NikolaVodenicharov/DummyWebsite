using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Enums.TyreEnums;

namespace Tyres.Data.Models.Orders
{
    public class Order : BaseCart
    {
        public OrderStatus Status { get; set; } = OrderStatus.Processing;

        public DateTime Date { get; set; } = DateTime.UtcNow;

        public List<OrderItem> Items { get; set; } = new List<OrderItem>();
    }
}
