using Microsoft.AspNetCore.Mvc;

namespace KuaforApp.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
