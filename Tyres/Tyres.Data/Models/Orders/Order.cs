using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Enums.TyreEnums;

namespace Tyres.Data.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime Date { get; set; }

        public IList<Item> Items { get; set; }
    }
}
