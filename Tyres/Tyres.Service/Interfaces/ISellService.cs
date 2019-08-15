using System.Collections.Generic;
using Tyres.Data.Models.Orders;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Service.Interfaces
{
    public interface ISellService
    {
        bool AddToCart(ItemDTO model, string userId);

        bool Ordering(string userId);

        CartDTO GetCart(string userId);

        OrderDetailsDTO GetOrder(int orderId);

        List<OrderSummaryDTO> GetOrders(string userId);

        void EnsureOrdersInitialized(string userId);
    }
}
