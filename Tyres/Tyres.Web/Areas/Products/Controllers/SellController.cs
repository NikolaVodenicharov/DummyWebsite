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
        private readonly IOrderService orderService;
        private readonly UserManager<User> userManager;

        public SellController(IOrderService orderService, UserManager<User> userManager)
        {
            this.orderService = orderService;
            this.userManager = userManager;
        }

        [HttpPost]      
        public async Task<IActionResult> AddProduct(ItemDTO model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var isSuccessfulAdded = await this.orderService.AddToCartAsync(model, this.GetUserId());
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
            var cart = await this.orderService.GetCartAsync(this.GetUserId());

            return View(cart);
        }

        public async Task<IActionResult> Ordering()
        {
            var isSuccessfulOrdered = await this.orderService.OrderingAsync(this.GetUserId());
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
            var model = await this.orderService.GetOrderAsync(orderId);

            return View(model);
        }

        public async Task<IActionResult> GetOrders()
        {
            var model = await this.orderService.GetOrdersAsync(this.GetUserId());

            return View(model);
        }

        private string GetUserId()
        {
            return this.userManager.GetUserId(User);
        }
    }
}