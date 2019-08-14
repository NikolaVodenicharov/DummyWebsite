using Microsoft.AspNetCore.Mvc;

namespace Tyres.Web.Areas.Useful.Controllers
{
    [Area("Useful")]
    public class ArticlesController : Controller
    {
        public IActionResult TyreSize()
        {
            return View();
        }
    }
}