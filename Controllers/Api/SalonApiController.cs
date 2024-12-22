using KuaforApp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuaforApp.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class SalonApiController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        public SalonApiController(ApplicationDbContext context)
        {
            _context = context;
        }

        // LINQ örneği: En çok hizmet sunan salonu getir
        [HttpGet("top-salon")]
        public async Task<IActionResult> GetTopSalon()
        {
            var topSalon = await _context.Salons
                .Select(s => new { Salon = s, ServiceCount = s.Services.Count })
                .OrderByDescending(x => x.ServiceCount)
                .FirstOrDefaultAsync();
            if (topSalon == null) return NotFound();

            return Ok(new { SalonName = topSalon.Salon.Name, ServiceCount = topSalon.ServiceCount });
        }

        // Tüm salonları getir
        [HttpGet("all")]
        public async Task<IActionResult> GetAllSalons()
        {
            var salons = await _context.Salons.Select(s => new {
                s.Id,
                s.Name,
                s.Address,
                Opening = s.OpeningHour.ToString(),
                Closing = s.ClosingHour.ToString()
            }).ToListAsync();

            return Ok(salons);
        }
    }
}
