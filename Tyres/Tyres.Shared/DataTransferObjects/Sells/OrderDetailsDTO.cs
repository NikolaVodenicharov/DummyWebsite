using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Data.Models.Orders;

namespace Tyres.Shared.DataTransferObjects.Sells
{
    public class OrderDetailsDTO
    {
        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string DeliveryAddress { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime Date { get; set; }

        public IList<ItemDTO> Items { get; set; }
    }
}
