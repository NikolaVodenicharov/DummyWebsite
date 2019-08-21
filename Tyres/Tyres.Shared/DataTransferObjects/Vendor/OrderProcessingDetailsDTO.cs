using System;
using System.Collections.Generic;
using Tyres.Data.Enums;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Shared.DataTransferObjects.Vendor
{
    public class OrderProcessingDetailsDTO
    {
        public int OrderId { get; set; }
        public DateTime Date { get; set; }


        public string UserFirstName { get; set; }
        public string UserLastName { get; set; }
        public string DeliveryAddress { get; set; }

        public OrderStatus Status { get; set; }

        public IList<ItemDTO> Items { get; set; }
    }
}
