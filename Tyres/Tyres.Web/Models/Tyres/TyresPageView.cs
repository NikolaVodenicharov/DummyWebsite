using Tyres.Service.Models;

namespace Tyres.Web.Models.Tyres
{
    public class TyresPageView : PageView<TyreSummary>
    {
        public TyreSearchForm Search { get; set; }
    }
}
