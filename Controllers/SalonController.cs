using KuaforApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuaforApp.Controllers
{
    public class SalonController : Controller
    {
        private readonly ApplicationDbContext _context;
        public SalonController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var salons = await _context.Salons.ToListAsync();
            return View(salons);
        }

        public async Task<IActionResult> Details(int id)
        {
            var salon = await _context.Salons.Include(s => s.Services).Include(s => s.Employees)
                .FirstOrDefaultAsync(s => s.Id == id);
            if (salon == null) return NotFound();
            return View(salon);
        }
    }
}
