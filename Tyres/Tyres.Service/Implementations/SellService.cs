using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Tyres.Data;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Data.Models;
using Tyres.Data.Models.Orders;
using Tyres.Data.Models.Products;
using Tyres.Products.Data.Models;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Service.Implementations
{
    public class SellService : AbstractService, ISellService
    {
        public SellService(TyresDbContext db, IMapper mapper) 
            : base(db, mapper)
        {
        }

        public void AddToCart(ItemDTO model, string userId)
        {
            if (!this.IsProductCorrect(model))
            {
                return;
            }

            if (!this.IsUserExist(userId))
            {
                return;
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
        }

        public CartDTO GetCart(string userId)
        {
            if (!this.IsUserExist(userId))
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

        public void Ordering(string userId)
        {
            if (!this.IsUserExist(userId))
            {
                return;
            }

            var orders = db
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.Orders)
                .FirstOrDefault();

            var cartOrder = orders.Last();
            cartOrder.Date = DateTime.UtcNow;
            cartOrder.Status = OrderStatus.Processing;
            cartOrder.DeliveryAddress = db.Users.Find(userId).DeliveryAddress;

            orders.Add(this.CreateCart(userId));

            db.SaveChanges();
        }

        public OrderDetailsDTO GetOrder(int orderId)
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

        public List<OrderSummaryDTO> GetOrders(string userId)
        {
            if (!this.IsUserExist(userId))
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
                .ToList();

            return ordersSummary;
        }

        public void EnsureOrdersInitialized(string userId)
        {
            if (!this.IsUserExist(userId))
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

            db.SaveChanges();
        }

        private bool IsProductCorrect(ItemDTO model)
        {
            var type = Assembly
                .GetAssembly(typeof(Tyre))
                .GetTypes()
                .Where(t => t.Name == model.ProductName)
                .FirstOrDefault();

            var product = db.Find(type, model.ProductId);

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
        private bool IsUserExist(string userId)
        {
            return db
                .Users
                .Any(u => u.Id == userId);
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
    }
}
