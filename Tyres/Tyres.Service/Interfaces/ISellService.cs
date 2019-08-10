using Tyres.Data.Models.Orders;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Service.Interfaces
{
    public interface ISellService
    {
        void AddToCart(CartItemDTO model, string userId);

        void Ordering(string userId);

        CartDTO GetCart(string userId);

       void EnsureOrdersInitialized(string userId);
    }
}
