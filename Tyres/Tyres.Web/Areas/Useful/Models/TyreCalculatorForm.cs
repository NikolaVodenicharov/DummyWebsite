using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Tyres.Data.Enums.TyreEnums;

namespace Tyres.Web.Areas.Useful.Models
{
    public class TyreCalculatorForm
    {
        public IEnumerable<SelectListItem> Widths { get; set; }

        public IEnumerable<SelectListItem> Ratios { get; set; }

        public IEnumerable<SelectListItem> RimDiameters { get; set; }
    }
}
