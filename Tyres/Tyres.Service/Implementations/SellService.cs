using AutoMapper;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using Tyres.Data;
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

        public void AddToCart(CartItemForm model, string userId)
        {
            var type = Assembly
                .GetAssembly(typeof(Tyre))
                .GetTypes()
                .Where(t => t.Name == model.ProductName)
                .FirstOrDefault();

            var product = db.Find(type, model.ProductId);

            if (product == null)
            {
                return; // not found ?
            }

            var castedProduct = (IProduct)product;

            if (castedProduct.Price != model.Price || castedProduct.Quantity < model.Quantity)
            {
                return; // mistake ?
            }

            var user = db.Users.Find(userId);
            if (user == null)
            {
                return; // mistake ?
            }


            var cart = db
                .Users
                .Where(u => u.Id == userId)
                .Select(u => u.Cart)
                .FirstOrDefault();

            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId
                };

                db.Carts.Add(cart);
                db.SaveChanges();

                user.Cart = cart;
            }



            var cartItem = new Item
            {
                ProductId = model.ProductId,
                ProductName = model.ProductName,
                Price = model.Price,
                Quantity = model.Quantity,
                CartId = cart.Id
            };

            db.Items.Add(cartItem);
            db.SaveChanges();
        }
    }
}
