using System;
using Tyres.Data.Enums;

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
