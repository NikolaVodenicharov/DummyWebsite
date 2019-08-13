using System.Collections.Generic;
using Tyres.Shared.DataTransferObjects.Sells;
using Microsoft.AspNetCore.Mvc.Rendering;
using Tyres.Data.Enums.TyreEnums;
using System;

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
        public IEnumerable<SelectListItem> Statuses { get; set; }


        public IList<ItemDTO> Items { get; set; }
    }
}
