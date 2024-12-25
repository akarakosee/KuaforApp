using Microsoft.AspNetCore.Mvc;

namespace KuaforApp.Areas.Admin.Controllers
{
    public class EmployeesController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
