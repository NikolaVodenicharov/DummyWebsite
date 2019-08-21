using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Threading.Tasks;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Constants;
using Tyres.Service.Interfaces;
using Tyres.Shared.DataTransferObjects.Tyres;
using Tyres.Web.Infrastructure;
using Tyres.Web.Infrastructure.ApplicationServices;
using Tyres.Web.Infrastructure.Extensions;

namespace Tyres.Web.Areas.Workers.Controllers
{
    [Area("Workers")]
    [Authorize(Roles = RoleConstants.Worker + ", " + RoleConstants.Administrator)]
    public class VendorController : Controller
    {
        private readonly IOrderService vendorService;
        private readonly ITyreService tyreService;

        public VendorController(IOrderService vendorService, ITyreService tyreService)
        {
            this.vendorService = vendorService;
            this.tyreService = tyreService;
        }

        public async Task<IActionResult> GetProcessingOrders(int page = PageConstants.DefaultPage)
        {
            var model = await this.vendorService.GetProcessingOrdersAsync(page);

            return View(model);
        }

        public async Task<IActionResult> GetProcessingOrderDetails(int orderId)
        {
            var model = await this.vendorService.GetProcessingOrderDetailsAsync(orderId);

            return View(model);
        }

        public async Task<IActionResult> ProcessingOrder(int orderId)
        {
            var isSuccessfulProcessed = await this.vendorService.ProcessingOrder(orderId);

            if (!isSuccessfulProcessed)
            {
                TempData.AddErrrorMessage("Adding product was unsuccessful.");
                return RedirectToAction(nameof(GetProcessingOrderDetails), new { orderId });
            }

            TempData.AddSuccessMessage("Adding product was successful.");

            return RedirectToAction(nameof(GetProcessingOrders));
        }

        public IActionResult CreateTyre()
        {
            var model = this.CreateTyreDTO();

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> CreateTyre(CreateTyreDTO<SelectListItem> tyreModel)
        {
            if (ModelState.IsValid)
            {
                var isCreated = await this.tyreService.Create<SelectListItem>(tyreModel);

                if (isCreated)
                {
                    TempData.AddSuccessMessage("Tyre was successful created.");
                }
                else
                {
                    TempData.AddErrrorMessage("Tyre service error.");
                }
            }
            else
            {
                TempData.AddErrrorMessage("Invalid input data.");
            }

            return RedirectToAction(nameof(CreateTyre));
        }

        private CreateTyreDTO<SelectListItem> CreateTyreDTO()
        {
            return new CreateTyreDTO<SelectListItem>
            {
                Widths = SelectListItemGenerator.GetEnumValuesSkipFirst<Width>(),
                Ratios = SelectListItemGenerator.GetEnumValuesSkipFirst<Ratio>(),
                Diameters = SelectListItemGenerator.GetEnumValuesSkipFirst<Diameter>(),
                Seasons = SelectListItemGenerator.GetEnumNamesValues<Season>(Season.All),
                Speeds = SelectListItemGenerator.GetEnumNamesValues<SpeedIndex>(SpeedIndex.H),
                Loads = SelectListItemGenerator.GetEnumNamesValues<LoadIndex>(true),
                FuelEfficients = SelectListItemGenerator.GetEnumNamesValues<FuelEfficient>(FuelEfficient.A),
                WetGrips = SelectListItemGenerator.GetEnumNamesValues<WetGrip>(WetGrip.A),
                Noices = SelectListItemGenerator.GetEnumValuesSkipFirst<Noice>(),
            };
        }
    }
}