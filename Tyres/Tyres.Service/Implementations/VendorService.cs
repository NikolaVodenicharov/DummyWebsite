using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Sells;
using Tyres.Shared.DataTransferObjects.Vendor;
using static Tyres.Service.Constants.PageConstants;

namespace Tyres.Service.Implementations
{
    public class VendorService : AbstractService, IVendorService
    {
        public VendorService(TyresDbContext db, IMapper mapper) 
            : base(db, mapper)
        {
        }

        public List<OrderProcessingDTO> GetProcessingOrders(int page = DefaultPage)
        {
            var orders = this.db
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
                .ToList();

            return orders;
        }

        public OrderProcessingDetailsDTO GetProcessingOrderDetails(int orderId)
        {
            var order = this.db
                .Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.User)
                .Include(o => o.Items)
                .FirstOrDefault();

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

        public void ChangeOrderStatus(int orderId, OrderStatus status)
        {
            var order = db.Orders.Find(orderId);
            order.Status = status;
            db.SaveChanges();
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
