using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Sells;
using Tyres.Shared.DataTransferObjects.Vendor;
using static Tyres.Service.Constants.PageConstants;

namespace Tyres.Service.Implementations
{
    public class OrderService : AbstractService, IOrderService
    {
        public OrderService(TyresDbContext db, IMapper mapper) 
            : base(db, mapper)
        {
        }

        public async Task<List<OrderProcessingDTO>> GetProcessingOrdersAsync(int page = DefaultPage)
        {
            var orders = await this.db
                .Orders
                .Where(o => o.Status == OrderStatus.Processing)
                .OrderBy(o => o.Date)
                .Skip(PageSize * (page - 1))
                .Take(PageSize)
                .Select(o => new OrderProcessingDTO
                {
                    Id = o.Id,
                    Date = o.Date,
                    DeliveryAddress = o.DeliveryAddress,
                    UserFirstName = o.User.FirstName,
                    UserLastName = o.User.LastName,
                })
                .ToListAsync();

            return orders;
        }

        public async Task<OrderProcessingDetailsDTO> GetProcessingOrderDetailsAsync(int orderId)
        {
            var order = await this.db
                .Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.User)
                .Include(o => o.Items)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return null;
            }

            var model = new OrderProcessingDetailsDTO
            {
                OrderId = orderId,
                UserFirstName = order.User.FirstName,
                UserLastName = order.User.LastName,
                DeliveryAddress = order.DeliveryAddress,
                Status = order.Status,
                Statuses = GetOrderStatusValuesItems(),
                Date = order.Date,
                Items = order.Items
                        .AsQueryable()
                        .ProjectTo<ItemDTO>(mapper.ConfigurationProvider)
                        .ToList()
            };

            return model;
        }

        public async Task ChangeOrderStatusAsync(int orderId, OrderStatus status)
        {
            var order = await db.Orders.FindAsync(orderId);
            order.Status = status;
            await db.SaveChangesAsync();
        }

        private List<SelectListItem> GetOrderStatusValuesItems()
        {
            var enumElements = Enum.GetValues(typeof(OrderStatus));
            var items = new List<SelectListItem>(enumElements.Length);

            foreach (var element in enumElements)
            {
                var enumValue = ((int)element).ToString();

                var item = new SelectListItem
                {
                    Text = element.ToString(),
                    Value = enumValue,
                };

                items.Add(item);
            }

            items.RemoveAt(0);

            return items;
        }
    }
}
