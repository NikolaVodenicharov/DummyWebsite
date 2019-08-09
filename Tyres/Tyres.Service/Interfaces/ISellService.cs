using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Service.Interfaces
{
    public interface ISellService
    {
        void AddToCart(CartItemForm model, string userId);
    }
}
