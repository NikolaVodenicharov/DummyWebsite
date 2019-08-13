using System;
using System.Collections.Generic;
using System.Text;

namespace Tyres.Shared.DataTransferObjects.Vendor
{
    public class OrderProcessingDTO
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }

        public string UserFirstName { get; set; }

        public string UserLastName { get; set; }

        public string DeliveryAddress { get; set; }
    }
}
