using System;
using System.Collections.Generic;
using System.Text;

namespace Tyres.Data.Models.Orders
{
    public abstract class BaseCart
    {
        public string Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }
    }
}
