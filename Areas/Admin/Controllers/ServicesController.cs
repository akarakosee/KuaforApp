using Microsoft.AspNetCore.Mvc;

namespace KuaforApp.Areas.Admin.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
