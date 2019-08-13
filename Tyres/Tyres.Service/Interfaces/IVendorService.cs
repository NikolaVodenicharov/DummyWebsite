using System.Collections.Generic;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Constants;
using Tyres.Shared.DataTransferObjects.Sells;
using Tyres.Shared.DataTransferObjects.Vendor;

namespace Tyres.Service.Interfaces
{
    public interface IVendorService
    {
        List<OrderProcessingDTO> GetProcessingOrders(int page = PageConstants.DefaultPage);

        OrderProcessingDetailsDTO GetProcessingOrderDetails(int orderId);

        void ChangeOrderStatus(int orderId, OrderStatus status);
    }
}
