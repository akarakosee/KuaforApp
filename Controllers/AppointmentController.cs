using KuaforApp.Data;
using KuaforApp.Models;
using KuaforApp.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KuaforApp.Controllers
{
    [Authorize]
    public class AppointmentController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public AppointmentController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var employees = await _context.Employees
                .Include(e => e.EmployeeServices)
                .ThenInclude(es => es.Service)
                .ToListAsync();

            var model = new AppointmentViewModel
            {
                Employees = employees.Select(e => new EmployeeSelectViewModel
                {
                    EmployeeId = e.Id,
                    EmployeeName = e.FullName
                }).ToList()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(AppointmentViewModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Lütfen oturum açın.";
                return RedirectToAction("Index", "Account");
            }

            if (model == null || model.EmployeeId == 0 || model.SelectedDate == default || model.SelectedTime == default)
            {
                TempData["ErrorMessage"] = "Tüm alanları doldurduğunuzdan emin olun.";
                return View(model);
            }

            var appointment = new Appointment
            {
                UserId = user.Id,
                EmployeeId = model.EmployeeId,
                Date = model.SelectedDate.Date,
                Time = model.SelectedTime,
                Notes = model.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu!";
            return RedirectToAction("Create");
        }

        private async Task<List<string>> GetAlternativeHours(int employeeId, DateTime date, TimeSpan requestedTime)
        {
            var results = new List<string>();
            var prevTime = requestedTime - TimeSpan.FromHours(1);
            if (prevTime.Hours >= 9 && prevTime.Hours <= 18)
            {
                bool prevTaken = await _context.Appointments.AnyAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.Date.Date == date.Date &&
                    a.Time == prevTime);
                if (!prevTaken) results.Add(prevTime.ToString(@"hh\:mm"));
            }

            var nextTime = requestedTime + TimeSpan.FromHours(1);
            if (nextTime.Hours >= 9 && nextTime.Hours <= 18)
            {
                bool nextTaken = await _context.Appointments.AnyAsync(a =>
                    a.EmployeeId == employeeId &&
                    a.Date.Date == date.Date &&
                    a.Time == nextTime);
                if (!nextTaken) results.Add(nextTime.ToString(@"hh\:mm"));
            }

            return results;
        }
    }
}
