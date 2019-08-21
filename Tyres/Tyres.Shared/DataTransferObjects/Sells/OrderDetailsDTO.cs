using System;
using System.Collections.Generic;
using Tyres.Data.Enums;

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
