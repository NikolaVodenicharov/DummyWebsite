using System;
using System.Collections.Generic;
using System.Text;
using Tyres.Data.Enums.TyreEnums;

namespace Tyres.Shared.DataTransferObjects.Sells
{
    public class OrderSummaryDTO
    {
        public int Id { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime Date { get; set; }

        public decimal Sum { get; set; }
    }
}
