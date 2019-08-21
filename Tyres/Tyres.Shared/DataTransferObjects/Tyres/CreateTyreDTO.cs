using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Tyres.Data.Enums.TyreEnums;
using static Tyres.Data.Constants.Validations.TyreValidationConstants;

namespace Tyres.Shared.DataTransferObjects.Tyres
{
    public class CreateTyreDTO<T>
    {
        [Required, MaxLength(BrandMaxLength)]
        public string Brand { get; set; }

        [Required, MaxLength(ModelMaxLength)]
        public string Model { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        [Required, Range(0, double.MaxValue)]
        public decimal Price { get; set; }

        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; set; }


        public Width Width { get; set; }
        public IEnumerable<T> Widths { get; set; }

        public Ratio Ratio { get; set; }
        public IEnumerable<T> Ratios { get; set; }

        public Diameter Diameter { get; set; }
        public IEnumerable<T> Diameters { get; set; }

        public Season Season { get; set; }
        public IEnumerable<T> Seasons { get; set; }

        [Display(Name = "Speed index")]
        public SpeedIndex Speed { get; set; }
        public IEnumerable<T> Speeds { get; set; }

        [Display(Name = "Load index")]
        public LoadIndex Load { get; set; }
        public IEnumerable<T> Loads { get; set; }

        [Display(Name = "Fuel efficient")]
        public FuelEfficient FuelEfficient { get; set; }
        public IEnumerable<T> FuelEfficients { get; set; }

        [Display(Name = "Wet grip")]
        public WetGrip WetGrip { get; set; }
        public IEnumerable<T> WetGrips { get; set; }

        public Noice Noice { get; set; }
        public IEnumerable<T> Noices { get; set; }
    }
}
