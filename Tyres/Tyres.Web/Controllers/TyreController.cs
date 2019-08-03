using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Interfaces;
using Tyres.Service.Models;
using Tyres.Web.Models;
using Tyres.Web.Models.Tyres;

namespace Tyres.Web.Controllers
{
    public class TyreController : Controller
    {
        private ITyreService tyreService;

        public TyreController(ITyreService tyreService)
        {
            this.tyreService = tyreService;
        }

        public IActionResult Index()
        {
            var model = this.GenerateTyreSearchForm();
            return View(model);
        }

        private TyreSearchForm GenerateTyreSearchForm ()
        {
            return new TyreSearchForm
            {
                Widths = GetEnumValuesItems<Width>(),
                Ratios = GetEnumValuesItems<Ratio>(),
                Diameters = GetEnumValuesItems<Diameter>(),
                Seasons = GetEnumNamesValuesItems<Season>(),
            };
        }

        // TODO: move these 2 methods in to the service ?

        /// <summary>
        /// Making Collection of SelectListItem from enumeration taking only the values. 
        /// If the name of first element is "All" takeFirstElementName = true. Else takeFirstElementName = false.
        /// </summary>
        private List<SelectListItem> GetEnumValuesItems<T>(bool takeFirstElementName = true) where T : Enum
        {
            var items = new List<SelectListItem>();

            foreach (var element in Enum.GetValues(typeof(T)))
            {
                var enumValue = ((int)element).ToString();

                items.Add(new SelectListItem
                    {
                        Text = enumValue,
                        Value = enumValue,
                    });
            }

            if (takeFirstElementName && items.Count > 0)
            {
                var enumValue = int.MinValue;

                var isSuccessful = int.TryParse(
                    items.First().Value, 
                    out enumValue);

                if (isSuccessful)
                {
                    var name = Enum.GetName(typeof(T), enumValue);
                    items.First().Text = name;
                }
            }

            return items;
        }

        private List<SelectListItem> GetEnumNamesValuesItems<T>() where T : Enum
        {
            var items = new List<SelectListItem>();

            foreach (var element in Enum.GetValues(typeof(T)))
            {
                var item = new SelectListItem
                {
                    Text = element.ToString(),
                    Value = ((int)element).ToString(),
                };

                items.Add(item);
            }

            return items;
        }

        public IActionResult All(TyreSearchForm model, int page = 1)
        {
            var tyreSearchForm = this.GenerateTyreSearchForm();
            tyreSearchForm.Widths.Where(i => i.Value == ((int)model.Width).ToString()).FirstOrDefault().Selected = true;
            tyreSearchForm.Ratios.Where(i => i.Value == ((int)model.Ratio).ToString()).FirstOrDefault().Selected = true;
            tyreSearchForm.Diameters.Where(i => i.Value == ((int)model.Diameter).ToString()).FirstOrDefault().Selected = true;
            tyreSearchForm.Seasons.Where(i => i.Value == ((int)model.Season).ToString()).FirstOrDefault().Selected = true;

            var tyresPage = new TyresPageView
            {
                Page = page,
                PagesCount = this.tyreService.GetPagesCount(model.Width, model.Ratio, model.Diameter, model.Season),
                Elements = this.tyreService.GetAllListing(model.Width, model.Ratio, model.Diameter, model.Season, page),
                Search = tyreSearchForm //this.GenerateTyreSearchForm()
            };

            //var tyresPage = new PageView<TyreSummary>
            //{
            //    Page = page,
            //    PagesCount = this.tyreService.GetPagesCount(model.Width, model.Ratio, model.Diameter, model.Season),
            //    Elements = this.tyreService.GetAllListing(model.Width, model.Ratio, model.Diameter, model.Season, page)
            //};

            return View(tyresPage);
        }
    }
}