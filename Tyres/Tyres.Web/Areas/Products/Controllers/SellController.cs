using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tyres.Data.Models;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Sells;
using Tyres.Web.Infrastructure.Extensions;

namespace Tyres.Web.Areas.Products.Controllers
{
    [Area("Products")]
    [Authorize]
    public class SellController : Controller
    {
        private readonly ISellService sellService;
        private readonly UserManager<User> userManager;

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

            var isSuccessfulAdded = this.sellService.AddToCart(model, this.GetUserId());
            if (!isSuccessfulAdded)
            {
                TempData.AddErrrorMessage("Adding product was unsuccessful.");
                return RedirectToAction("Index", "Tyre");
            }

            TempData.AddSuccessMessage("Adding product was successful.");
            return RedirectToAction(nameof(GetCart));
        }

        public IActionResult GetCart()
        {
            var cart = this.sellService.GetCart(this.GetUserId());

            return View(cart);
        }

        public IActionResult Ordering()
        {
            var isSuccessfulOrdered = this.sellService.Ordering(this.GetUserId());
            if (!isSuccessfulOrdered)
            {
                TempData.AddErrrorMessage("Ordering was unsuccessful.");
                return RedirectToAction("Index", "Tyre");
            }

            TempData.AddSuccessMessage("Ordering was successful.");
            return RedirectToAction(nameof(GetOrders));
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