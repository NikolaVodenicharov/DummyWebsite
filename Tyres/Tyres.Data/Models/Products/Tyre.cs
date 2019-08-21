using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Data.Models.Products;
using static Tyres.Data.Constants.Validations.TyreValidationConstants;

namespace Tyres.Data.Models.Products
{
    public class Tyre : IProduct
    {
        public int Id { get; set; }

        [Required, MaxLength(BrandMaxLength)]
        public string Brand { get; set; }

        [Required, MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Range(0, int.MaxValue)]
        public int Quantity { get; set; }

        public Width Width { get; set; }
        public Ratio Ratio { get; set; }
        public Diameter Diameter { get; set; }
        public Season Season { get; set; }
        public SpeedIndex Speed { get; set; }
        public LoadIndex Load { get; set; }
        public FuelEfficient FuelEfficient { get; set; }
        public WetGrip WetGrip { get; set; }
        public Noice Noice { get; set; }
    }
}
