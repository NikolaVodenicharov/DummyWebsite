using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using Tyres.Data.Enums.TyreEnums;
using Tyres.Web.Areas.Useful.Models;

namespace Tyres.Web.Areas.Useful.Controllers
{
    [Area("Useful")]
    public class TyreCalculatorController : Controller
    {
        public IActionResult Calculator()
        {
            var model = new TyreCalculatorForm
            {
                Widths = GetEnumValuesItems<Width>(Width._195, true),
                Ratios = GetEnumValuesItems<Ratio>(Ratio._65, true),
                RimDiameters = GetEnumValuesItems<Diameter>(Diameter._15, true)
            };

            return View(model);
        }

        //TODO extract method to some "Logic" project or folder
        private List<SelectListItem> GetEnumValuesItems<T>(T selected, bool skipFirstElement = false) where T : Enum
        {
            var enumElements = Enum.GetValues(typeof(T));
            var items = new List<SelectListItem>(enumElements.Length);

            var isSelectedItemFound = false;

            foreach (var element in enumElements)
            {
                if (skipFirstElement)
                {
                    skipFirstElement = false;
                    continue;
                }

                var enumValue = ((int)element).ToString();

                var item = new SelectListItem
                {
                    Text = enumValue,
                    Value = enumValue,
                };

                if (!isSelectedItemFound && selected.Equals(element))
                {
                    item.Selected = true;
                    isSelectedItemFound = true;
                }

                items.Add(item);
            }

            return items;
        }
    }
}