using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Constants;
using Tyres.Service.Interfaces;
using Tyres.Web.Infrastructure;

namespace Tyres.Web.Areas.Workers.Controllers
{
    [Area("Workers")]
    [Authorize(Roles = RoleConstants.Worker + ", " + RoleConstants.Administrator)]
    public class VendorController : Controller
    {
        private readonly IVendorService vendorService;

        public VendorController(IVendorService vendorService)
        {
            this.vendorService = vendorService;
        }

        public IActionResult GetProcessingOrders(int page = PageConstants.DefaultPage)
        {
            var model = this.vendorService.GetProcessingOrders(page);

            return View(model);
        }

        public IActionResult GetProcessingOrderDetails(int orderId)
        {
            var model = this.vendorService.GetProcessingOrderDetails(orderId);

            return View(model);
        }

        [HttpPost]
        public IActionResult ChangeOrderStatus(int orderId, OrderStatus status)
        {
            this.vendorService.ChangeOrderStatus(orderId, status);

            return RedirectToAction(nameof(GetProcessingOrders));
        }


    }
}