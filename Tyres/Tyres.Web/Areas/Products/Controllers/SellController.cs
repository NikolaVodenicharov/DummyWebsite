using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tyres.Data.Models;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Web.Areas.Products.Controllers
{
    [Area("Products")]
    [Authorize]
    public class SellController : Controller
    {
        private ISellService sellService;
        private UserManager<User> userManager;

        public SellController(ISellService sellService, UserManager<User> userManager)
        {
            this.sellService = sellService;
            this.userManager = userManager;
        }

        [HttpPost]      
        public IActionResult AddProduct(ItemDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            this.sellService.AddToCart(model, this.GetUserId());

            return RedirectToAction(nameof(GetCart));
        }

        public IActionResult GetCart()
        {
            var cart = this.sellService.GetCart(this.GetUserId());

            return View(cart);
        }

        public IActionResult Ordering()
        {
            this.sellService.Ordering(this.GetUserId());

            return RedirectToAction("index", "Tyre");
        }

        public IActionResult GetOrder(int orderId)
        {
            var model = this.sellService.GetOrder(orderId);

            return View(model);
        }

        public IActionResult GetOrders()
        {
            var model = this.sellService.GetOrders(this.GetUserId());

            return View(model);
        }

        private string GetUserId()
        {
            return this.userManager.GetUserId(User);
        }
    }
}