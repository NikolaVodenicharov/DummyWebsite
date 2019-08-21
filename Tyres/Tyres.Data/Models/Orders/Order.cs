using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tyres.Data.Constants.Validations;
using Tyres.Data.Enums;

namespace Tyres.Data.Models.Orders
{
    public class Order
    {
        public int Id { get; set; }

        public User User { get; set; }
        public string UserId { get; set; }

        public OrderStatus Status { get; set; }

        public DateTime Date { get; set; }

        [MaxLength(UserValidationConstants.DeliveryAddressMaxlenght)]
        public string DeliveryAddress { get; set; }

        public IList<Item> Items { get; set; }
    }
}
