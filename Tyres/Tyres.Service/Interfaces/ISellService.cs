using System.Collections.Generic;
using System.Threading.Tasks;
using Tyres.Data.Models.Orders;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Service.Interfaces
{
    public interface ISellService
    {
        Task<bool> AddToCartAsync(ItemDTO model, string userId);

        Task<bool> OrderingAsync(string userId);

        Task<CartDTO> GetCartAsync(string userId);

        Task<OrderDetailsDTO> GetOrderAsync(int orderId);

        Task<List<OrderSummaryDTO>> GetOrdersAsync(string userId);

        Task EnsureOrdersInitializedAsync(string userId);
    }
}
