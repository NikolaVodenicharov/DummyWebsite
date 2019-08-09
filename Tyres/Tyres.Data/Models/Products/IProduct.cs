using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Tyres.Data.Models.Products
{
    public interface IProduct
    {
        int Id { get; set; }

        string ProductName { get; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        int Quantity { get; set; }
    }
}
