using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Constants;
using Tyres.Service.Interfaces;
using Tyres.Web.Areas.Products.Models.Tyres;

namespace Tyres.Web.Areas.Products.Controllers
{
    [Area("Products")]
    public class TyreController : Controller
    {
        private readonly ITyreService tyreService;
        private readonly IMemoryCache memoryCache;

        public TyreController(ITyreService tyreService, IMemoryCache memoryCache)
        {
            this.tyreService = tyreService;
            this.memoryCache = memoryCache;
        }

        public IActionResult Index()
        {
            var model = this.memoryCache.GetOrCreate("TyreSearchForm", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(12);
                return this.GenerateTyreSearchForm();
            });


            //var model = this.GenerateTyreSearchForm();


            return View(model);
        }

        public async Task<IActionResult> All(TyreSearchForm model, int page = PageConstants.DefaultPage)
        {
            var pageCountTask = this.tyreService.GetPagesCount(model.Width, model.Ratio, model.Diameter, model.Season);
            var tyres = await this.tyreService.GetAllListingAsync(model.Width, model.Ratio, model.Diameter, model.Season, page);

            var tyreSearchForm = this.GenerateTyreSearchForm(model.Width, model.Ratio, model.Diameter, model.Season);


            var tyresPage = new TyresPageView
            {
                Page = page,
                PagesCount = await pageCountTask,
                Elements = tyres.ToList(),
                Search = tyreSearchForm
            };

            return View(tyresPage);
        }

        public async Task<IActionResult> Details(int id)
        {
            var model = await this.tyreService.Get(id);
            return View(model);
        }

        private TyreSearchForm GenerateTyreSearchForm(Width selectedWidth = 0, Ratio selectedRatio = 0, Diameter selectedDiameter = 0, Season selectedSeason = 0)
        {
            return new TyreSearchForm
            {
                Widths = GetEnumValuesItems<Width>(Width._195),
                Ratios = GetEnumValuesItems<Ratio>(selectedRatio),
                Diameters = GetEnumValuesItems<Diameter>(selectedDiameter),
                Seasons = GetEnumNamesValuesItems<Season>(selectedSeason)
            };
        }


        // TODO: move these 2 methods in to the service  or to some "Logic" project or folder?

        /// <summary>
        /// Making Collection of SelectListItem from enumeration taking only the values. 
        /// We have to set what is the default selected value.
        /// If the name of first element is "All" takeFirstElementName = true. Else takeFirstElementName = false.
        /// </summary>
        private List<SelectListItem> GetEnumValuesItems<T>(T selected, bool requireFirstElementName = true) where T : Enum
        {
            var enumElements = Enum.GetValues(typeof(T));
            var items = new List<SelectListItem>(enumElements.Length);

            var isFirstElement = true;
            var isSearchingSelectedItem = true;

            foreach (var element in enumElements)
            {
                var enumValue = ((int)element).ToString();

                var item = new SelectListItem
                {
                    Text = enumValue,
                    Value = enumValue,
                };

                if (isFirstElement && requireFirstElementName)
                {
                    item.Text = element.ToString();
                    isFirstElement = false;
                }

                if (isSearchingSelectedItem && selected.Equals(element))
                {
                    item.Selected = true;
                    isSearchingSelectedItem = false;
                }

                items.Add(item);
            }

            return items;
        }

        private List<SelectListItem> GetEnumNamesValuesItems<T>(T selected) where T : Enum
        {
            var enumElements = Enum.GetValues(typeof(T));
            var items = new List<SelectListItem>(enumElements.Length);
            var isSearchingSelectedItem = true;

            foreach (var element in enumElements)
            {
                var item = new SelectListItem
                {
                    Text = element.ToString(),
                    Value = ((int)element).ToString(),
                };

                if (isSearchingSelectedItem && selected.Equals(element))
                {
                    item.Selected = true;
                    isSearchingSelectedItem = false;
                }

                items.Add(item);
            }

            return items;
        }
    }
}