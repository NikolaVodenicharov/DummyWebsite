using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Data.Models;
using Tyres.Data.Models.Orders;
using Tyres.Data.Models.Products;
using Tyres.Products.Data.Models;
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

        public async Task<bool> AddToCartAsync(ItemDTO model, string userId)
        {
            var isProductCorrectTask = this.IsProductCorrectAsync(model);
            var isUserExistTask = this.IsUserExistAsync(userId);

            if (!await isProductCorrectTask || !await isUserExistTask)
            {
                return false;
            }

            var notOrdered = db
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u
                    .Orders
                    .FirstOrDefault(o => o.Status == OrderStatus.NotOrdered))
                .FirstOrDefault();

            var cartItem = new Item
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                Price = model.Price,
                Quantity = model.Quantity,
                OrderId = notOrdered.Id
            };

            db.Items.Add(cartItem);
            db.SaveChanges();

            return true;
        }

        public async Task<CartDTO> GetCartAsync(string userId)
        {
            if (!await this.IsUserExistAsync(userId))
            {
                return null;
            }

            var orderWithItems = db
                .Orders
                .Where(u =>
                    u.UserId == userId &&
                    u.Status == OrderStatus.NotOrdered)
                .Include(o => o.Items)
                .FirstOrDefault();

            var isCartExist = orderWithItems != null;
            if (!isCartExist)
            {
                var user = GetUserWithOrders(userId);
                orderWithItems = this.CreateCart(userId);
                user.Orders.Add(orderWithItems);

                db.SaveChanges();
            }

            var cart = new CartDTO
            {
                Id = orderWithItems.Id,
                UserId = orderWithItems.UserId,
                Items = orderWithItems
                    .Items
                    .AsQueryable()
                    .ProjectTo<ItemDTO>(mapper.ConfigurationProvider)
                    .ToList()
            };

            return cart;
        }

        public async Task<bool> OrderingAsync(string userId)
        {
            if (!await this.IsUserExistAsync(userId))
            {
                return false;
            }

            var userTask = db.Users.FindAsync(userId);

            var orders = await db
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.Orders)
                .FirstOrDefaultAsync();

            var cartOrder = orders.Last();
            cartOrder.Date = DateTime.UtcNow;
            cartOrder.Status = OrderStatus.Processing;
            cartOrder.DeliveryAddress = (await userTask).DeliveryAddress;

            orders.Add(this.CreateCart(userId));

            await db.SaveChangesAsync();

            return true;
        }

        public async Task<OrderDetailsDTO> GetOrderAsync(int orderId)
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

            var model = new OrderDetailsDTO
            {
                UserFirstName = order.User.FirstName,
                UserLastName = order.User.LastName,
                DeliveryAddress = order.DeliveryAddress,
                Status = order.Status,
                Date = order.Date,
                Items = order.Items
                        .AsQueryable()
                        .ProjectTo<ItemDTO>(mapper.ConfigurationProvider)
                        .ToList()
            };

            return model;
        }

        public async Task<List<OrderSummaryDTO>> GetOrdersAsync(string userId)
        {
            if (!await this.IsUserExistAsync(userId))
            {
                return null;
            }

            var ordersSummary = db
                .Orders
                .Where(o => o.UserId == userId && o.Status != OrderStatus.NotOrdered)
                .Select(o => new OrderSummaryDTO
                {
                    Id = o.Id,
                    Date = o.Date,
                    Status = o.Status,
                    Sum = o.Items.Sum(i => i.Price)
                })
                .ToListAsync();

            return await ordersSummary;
        }

        public async Task EnsureOrdersInitializedAsync(string userId)
        {
            var isUserExist = await this.IsUserExistAsync(userId);
            if (!isUserExist)
            {
                return;
            }

            var user = GetUserWithOrders(userId);

            if (user.Orders == null)
            {
                user.Orders = new List<Order>();
            }

            if (user.Orders.Count == 0)
            {
                user.Orders.Add(this.CreateCart(userId));
            }

            await db.SaveChangesAsync();
        }

        private async Task<bool> IsProductCorrectAsync(ItemDTO model)
        {
            var type = Assembly
                .GetAssembly(typeof(Tyre))
                .GetTypes()
                .Where(t => t.Name == model.ProductName)
                .FirstOrDefault();

            var product = await db.FindAsync(type, model.ProductId);

            var isProductExist = product != null;
            if (!isProductExist)
            {
                return false;
            }

            var castedProduct = (IProduct)product;

            var isPriceCorrect = castedProduct.Price == model.Price;
            var isQuantityAvailable = castedProduct.Quantity >= model.Quantity;
            if (!isPriceCorrect || !isQuantityAvailable)
            {
                return false;
            }

            return true;
        }
        private async Task<bool> IsUserExistAsync(string userId)
        {
            return await db
                .Users
                .AnyAsync(u => u.Id == userId);
        }

        private User GetUserWithOrders(string userId)
        {
            return db
                .Users
                .Where(u => u.Id == userId)
                .Include(u => u.Orders)
                .FirstOrDefault();
        }
        private Order CreateCart(string userId)
        {
            return new Order
            {
                UserId = userId,
                Status = OrderStatus.NotOrdered,
                Items = new List<Item>()
            };
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
