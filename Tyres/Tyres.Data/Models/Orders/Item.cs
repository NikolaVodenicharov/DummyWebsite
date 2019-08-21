using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tyres.Data.Enums;

namespace Tyres.Data.Models.Orders
{
    public class Item
    {
        public int Id { get; set; }

        public int ProductId { get; set; }

        public ProductType ProductType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }


        public Order Order { get; set; }
        public int OrderId { get; set; }
    }
}
