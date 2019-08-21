using Tyres.Data.Enums;
using Tyres.Data.Models.Orders;
using Tyres.Shared.Mappings;

namespace Tyres.Shared.DataTransferObjects.Sells
{
    public class ItemDTO : IMapFrom<Item>
    {
        public int ProductId { get; set; }

        public ProductType ProductType { get; set; }

        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}
