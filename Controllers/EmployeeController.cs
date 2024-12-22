using Microsoft.AspNetCore.Mvc;

namespace KuaforApp.Controllers
{
    public class EmployeeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
