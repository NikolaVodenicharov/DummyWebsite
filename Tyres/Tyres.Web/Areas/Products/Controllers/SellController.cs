using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Tyres.Data.Models;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Sells;

namespace Tyres.Web.Areas.Products.Controllers
{
    [Area("Products")]
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
        [Authorize]
        public IActionResult AddProduct(CartItemForm model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var userId = this.userManager.GetUserId(User);

            this.sellService.AddToCart(model, userId);

            return RedirectToAction("index", "Tyre");
        }
    }
}