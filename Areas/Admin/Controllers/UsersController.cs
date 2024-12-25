using Microsoft.AspNetCore.Mvc;

namespace KuaforApp.Areas.Admin.Controllers
{
    public class UsersController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
