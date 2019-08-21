using Tyres.Data.Enums;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Data.Models.Products;
using Tyres.Shared.Mappings;

namespace Tyres.Shared.DataTransferObjects.Tyres
{
    public class TyreDetailsDTO : IMapFrom<Tyre>
    {
        public int Id { get; set; }

        public ProductType ProductType { get; } = ProductType.Tyre;

        public string Brand { get; set; }

        public string Model { get; set; }

        public decimal Price { get; set; }

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
