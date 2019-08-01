using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tyres.Data.Enums.TyreEnums;

namespace Tyres.Web.Models.Tyres
{
    public class TyreSearch
    {
        public Width Width { get; set; }
        public List<SelectListItem> Widths { get; set; }

        public Ratio Ratio { get; set; }
        public List<SelectListItem> Ratios { get; set; }

        public Diameter Diameter { get; set; }
        public List<SelectListItem> Diameters { get; set; }

        public Season Season { get; set; }
        public List<SelectListItem> Seasons { get; set; }

    }
}
