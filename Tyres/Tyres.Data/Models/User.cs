using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Tyres.Data.Models.Orders;
using static Tyres.Data.Constants.Validations.UserValidationConstants;

namespace Tyres.Data.Models
{
    public class User : IdentityUser
    {
        [Required, MaxLength(FirstNameMaxlength)]
        public string FirstName { get; set; }

        [Required, MaxLength(LastNameMaxlength)]
        public string LastName { get; set; }

        [Required, MaxLength(DeliveryAddressMaxlenght)]
        public string DeliveryAddress { get; set; }

        public IList<Order> Orders { get; set; }
    }
}
