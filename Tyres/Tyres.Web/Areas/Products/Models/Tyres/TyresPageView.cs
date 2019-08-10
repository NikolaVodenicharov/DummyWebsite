using Tyres.Shared.DataTransferObjects.Tyres;
using Tyres.Web.Models;

namespace Tyres.Web.Areas.Products.Models.Tyres
{
    public class TyresPageView : PageView<TyreSummaryDTO>
    {
        public TyreSearchForm Search { get; set; }
    }
}
