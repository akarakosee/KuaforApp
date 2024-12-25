using Microsoft.AspNetCore.Mvc;

namespace KuaforApp.Areas.Admin.Controllers
{
    public class SalonsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
