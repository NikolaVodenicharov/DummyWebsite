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
    }
}
