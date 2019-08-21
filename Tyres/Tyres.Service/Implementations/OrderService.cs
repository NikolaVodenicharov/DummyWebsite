using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tyres.Data;
using Tyres.Data.Enums;
using Tyres.Data.Models;
using Tyres.Data.Models.Orders;
using Tyres.Data.Models.Products;
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

            if (!isProductCorrectTask || !await isUserExistTask)
            {
                return false;
            }

            var cart = db
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u
                    .Orders
                    .Last())
                .FirstOrDefault();

            if (cart.Status != OrderStatus.NotOrdered)
            {
                return false;
            }


            var cartItem = new Item
            {
                ProductId = model.ProductId,
                ProductType = model.ProductType,
                Price = model.Price,
                Quantity = model.Quantity,
                OrderId = cart.Id
            };

            db.Items.Add(cartItem);
            await db.SaveChangesAsync();

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
            var deliveryAddress = await db
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.DeliveryAddress)
                .FirstOrDefaultAsync();

            var isUserCorrect = deliveryAddress != null;
            if (!isUserCorrect)
            {
                return false;
            }

            var orders = await db
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.Orders)
                .FirstOrDefaultAsync();

            var cart = await EnsureCartExistAsync(orders, userId);

            if (await IsOrderEmpty(cart.Id))
            {
                return false;
            }

            TransformCartToOrder(deliveryAddress, cart);
            AddNewCart(userId, orders);

            await db.SaveChangesAsync();

            return true;
        }
        private async Task<Order> EnsureCartExistAsync(IList<Order> orders, string userId)
        {
            var anyOrder = orders.Count > 0;
            if (anyOrder)
            {
                var lastOrder = orders.Last();

                var isCart = lastOrder.Status == OrderStatus.NotOrdered;
                if (isCart)
                {
                    return lastOrder;
                }
            }

            var cart = this.CreateCart(userId);
            orders.Add(cart);
            await db.SaveChangesAsync();

            return cart;
        }
        private async Task<bool> IsOrderEmpty(int orderId)
        {
            var itemsCount = await db
                .Orders
                .Where(o => o.Id == orderId)
                .Select(o => o.Items.Count)
                .FirstOrDefaultAsync();

            var isEmpy = itemsCount == 0;

            return isEmpy;
        }
        private static void TransformCartToOrder(string deliveryAddress, Order cart)
        {
            cart.Date = DateTime.UtcNow;
            cart.Status = OrderStatus.Processing;
            cart.DeliveryAddress = deliveryAddress;
        }
        private void AddNewCart(string userId, IList<Order> orders)
        {
            var cart = this.CreateCart(userId);
            orders.Add(cart);
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
                    Sum = o.Items.Sum(i => i.Price * i.Quantity)
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

        private bool IsProductCorrectAsync(ItemDTO model)
        {
            var product = this.FindProduct(model.ProductType, model.ProductId);

            var isProductExist = product != null;
            if (!isProductExist)
            {
                return false;
            }

            var isPriceCorrect = product.Price == model.Price;
            var isQuantityAvailable = product.Quantity >= model.Quantity;
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

        public async Task<bool> ProcessingOrder(int orderId) // SendProducts ?
        {
            var isSuccess = this.DecreaseProductsQuantity(orderId);
            if (!isSuccess)
            {
                return false;
            }

            await this.ChangeOrderStatusAsync(orderId, OrderStatus.Shipping);

            return true;
        }

        private bool DecreaseProductsQuantity(int orderId)
        {
            var order = db
                .Orders
                .Where(o => o.Id == orderId)
                .Include(o => o.Items)
                .FirstOrDefault();

            if (order == null)
            {
                return false;
            }

            foreach (var item in order.Items)
            {
                var product = this.FindProduct(item.ProductType, item.ProductId);

                var isProductExist = product != null;
                if (!isProductExist)
                {
                    return false;
                }

                var isQuantityAvailable = product.Quantity >= item.Quantity;
                if (!isQuantityAvailable)
                {
                    return false;
                }

                product.Quantity -= item.Quantity;
            }

            db.SaveChanges();

            return true;
        }

        private IProduct FindProduct(ProductType productType, int productId)
        {
            IProduct product = null;

            switch (productType)
            {
                //TODO: test the time
                case ProductType.Tyre:
                    product = TestFindProduct(db.Tyres.Cast<IProduct>(), productId);
                    break;
                default:
                    break;
            }
        
            return product;
        }
        private IProduct TestFindProduct(IQueryable<IProduct> dbSet, int productId)
        {
            var result = dbSet
                .Where(p => p.Id == productId)
                .FirstOrDefault();

            return result;
        }
    }
}
