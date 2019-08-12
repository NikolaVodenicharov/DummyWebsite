using System.Collections.Generic;
using Tyres.Data.Models.Orders;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Service.Interfaces
{
    public interface ISellService
    {
        void AddToCart(ItemDTO model, string userId);

        void Ordering(string userId);

        CartDTO GetCart(string userId);

        OrderDetailsDTO GetOrder(int orderId);

        List<OrderSummaryDTO> GetOrders(string userId);

        void EnsureOrdersInitialized(string userId);
    }
}
