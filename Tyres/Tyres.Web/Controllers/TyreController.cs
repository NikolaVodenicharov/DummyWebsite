using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Service.Interfaces;
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

        public IActionResult Search()
        {
            //var widths = new SelectList(Enum.GetValues(typeof(Width))
            //    .Cast<Width>()
            //    .Select(v => new SelectListItem
            //    {
            //        Text = v.ToString(),
            //        Value = = ((int)v).ToString()
            //    })
            //    .ToList(), "Text", "Value");

            var model = new TyreSearch
            {
                Widths = GetEnumValues<Width>(),
                Ratios = GetEnumValues<Ratio>(),
                Diameters = GetEnumValues<Diameter>(),
                Seasons = GetEnumNamesValues<Season>(),
            };
 
            return View(model);
        }

        /// <summary>
        /// Making Collection of SelectListItem from enumeration taking only the values. 
        /// If the name of first element is "All" takeFirstElementName = true. Else takeFirstElementName = false.
        /// </summary>
        private List<SelectListItem> GetEnumValues<T>(bool takeFirstElementName = true) where T : Enum
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

        private List<SelectListItem> GetEnumNamesValues<T>() where T : Enum
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

        [HttpPost]
        public IActionResult Search(TyreSearch model)
        {
            var listedTyres = this.tyreService.AllListing(model.Width, model.Ratio, model.Diameter, model.Season);

            return View();
        }
    }
}