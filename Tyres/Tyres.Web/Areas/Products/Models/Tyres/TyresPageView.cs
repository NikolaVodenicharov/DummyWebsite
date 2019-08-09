using System.Collections.Generic;
using Tyres.Service.Models;
using Tyres.Web.Models;

namespace Tyres.Web.Areas.Products.Models.Tyres
{
    public class TyresPageView : PageView<TyreSummary>
    {
        public TyreSearchForm Search { get; set; }
    }
}
