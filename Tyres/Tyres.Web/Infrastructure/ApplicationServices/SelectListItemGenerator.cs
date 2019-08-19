using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;

namespace Tyres.Web.Infrastructure.ApplicationServices
{
    public static class SelectListItemGenerator
    {

        public static List<SelectListItem> GetEnumValuesSkipFirst<T>(bool skipFirstElement = true) where T : Enum
        {
            var items = GetEnumValues<T>();
            if (skipFirstElement)
            {
                items.RemoveAt(0);
            }

            return items;
        }

        public static List<SelectListItem> GetEnumValuesSkipFirst<T>(T selected, bool skipFirstElement = true) where T : Enum
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
       
        public static List<SelectListItem> GetEnumValuesFirstElementName<T>(T selected, bool requireFirstElementName = true) where T : Enum
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

        public static List<SelectListItem> GetEnumNamesValues<T>(T selected) where T : Enum
        {
            var enumElements = Enum.GetValues(typeof(T));
            var items = new List<SelectListItem>(enumElements.Length);
            var isSearchingItemSelected = false;

            foreach (var element in enumElements)
            {
                var item = new SelectListItem
                {
                    Text = element.ToString(),
                    Value = ((int)element).ToString(),
                };

                if (!isSearchingItemSelected && selected.Equals(element))
                {
                    item.Selected = true;
                    isSearchingItemSelected = true;
                }

                items.Add(item);
            }

            return items;
        }

        public static List<SelectListItem> GetEnumNamesValues<T>(bool removeNameFirstSymbol = false) where T : Enum
        {
            var enumElements = Enum.GetValues(typeof(T));
            var items = new List<SelectListItem>(enumElements.Length);

            foreach (var element in enumElements)
            {
                var text = element.ToString();
                if (removeNameFirstSymbol)
                {
                    text = text.Remove(0, 1);
                }

                var item = new SelectListItem
                {
                    Text = text,
                    Value = ((int)element).ToString(),
                };

                items.Add(item);
            }

            return items;
        }

        private static List<SelectListItem> GetEnumValues<T>() where T : Enum
        {
            var enumElements = Enum.GetValues(typeof(T));
            var items = new List<SelectListItem>(enumElements.Length);

            foreach (var element in enumElements)
            {
                var enumValue = ((int)element).ToString();

                var item = new SelectListItem
                {
                    Text = enumValue,
                    Value = enumValue,
                };

                items.Add(item);
            }

            return items;
        }
    }
}
