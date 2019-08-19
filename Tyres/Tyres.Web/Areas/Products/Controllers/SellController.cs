using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
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
        public async Task<IActionResult> AddProduct(ItemDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isSuccessfulAdded = await this.sellService.AddToCartAsync(model, this.GetUserId());
            if (!isSuccessfulAdded)
            {
                TempData.AddErrrorMessage("Adding product was unsuccessful.");
                return RedirectToAction("Index", "Tyre");
            }

            TempData.AddSuccessMessage("Adding product was successful.");
            return RedirectToAction(nameof(GetCart));
        }

        public async Task<IActionResult> GetCart()
        {
            var cart = await this.sellService.GetCartAsync(this.GetUserId());

            return View(cart);
        }

        public async Task<IActionResult> Ordering()
        {
            var isSuccessfulOrdered = await this.sellService.OrderingAsync(this.GetUserId());
            if (!isSuccessfulOrdered)
            {
                TempData.AddErrrorMessage("Ordering was unsuccessful.");
                return RedirectToAction("Index", "Tyre");
            }

            TempData.AddSuccessMessage("Ordering was successful.");
            return RedirectToAction(nameof(GetOrders));
        }

        public async Task<IActionResult> GetOrder(int orderId)
        {
            var model = await this.sellService.GetOrderAsync(orderId);

            return View(model);
        }

        public async Task<IActionResult> GetOrders()
        {
            var model = await this.sellService.GetOrdersAsync(this.GetUserId());

            return View(model);
        }

        private string GetUserId()
        {
            return this.userManager.GetUserId(User);
        }
    }
}