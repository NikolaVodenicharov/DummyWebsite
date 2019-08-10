using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Models.Orders;
using Tyres.Shared.Mappings;

namespace Tyres.Shared.DataTransferObjects.Sells
{
    public class CartDTO
    {
        public int Id { get; set; }

        public string UserId { get; set; }

        public IList<CartItemDTO> Items { get; set; }
    }
}
