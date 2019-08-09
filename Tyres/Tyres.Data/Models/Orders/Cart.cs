using System;
using System.Collections.Generic;
using System.Text;

namespace Tyres.Data.Models.Orders
{
    public class Cart
    {
        public int Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public IEnumerable<Item> Items { get; set; } = new List<Item>();
    }
}
