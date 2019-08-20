using System.Collections.Generic;
using System.Threading.Tasks;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Constants;
using Tyres.Shared.DataTransferObjects.Sells;
using Tyres.Shared.DataTransferObjects.Vendor;

namespace Tyres.Service.Interfaces
{
    public interface IOrderService
    {
        Task<List<OrderProcessingDTO>> GetProcessingOrdersAsync(int page = PageConstants.DefaultPage);

        Task<OrderProcessingDetailsDTO> GetProcessingOrderDetailsAsync(int orderId);

        Task ChangeOrderStatusAsync(int orderId, OrderStatus status);

        Task<bool> AddToCartAsync(ItemDTO model, string userId);

        Task<bool> OrderingAsync(string userId);

        Task<CartDTO> GetCartAsync(string userId);

        Task<OrderDetailsDTO> GetOrderAsync(int orderId);

        Task<List<OrderSummaryDTO>> GetOrdersAsync(string userId);

        Task EnsureOrdersInitializedAsync(string userId);
    }
}
