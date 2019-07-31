using System;
using System.Collections.Generic;
using System.Text;

namespace Tyres.Data.Models.Orders
{
    public class CartItem : BaseItem
    {
        public Cart Cart { get; set; }
        public string CartId { get; set; }
    }
}
