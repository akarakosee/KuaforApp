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
            // Berberleri (Employees) çek
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
            // Not alanı isteğe bağlı, bu yüzden null veya boş olabilir.
            // Gerekirse model.Notes’i kontrol etmeden kaydederiz.

            // Kullanıcı kimliğini al
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                // Oturum açmamışsa hata mesajı
                TempData["ErrorMessage"] = "Lütfen giriş yapınız.";
                return RedirectToAction("Login", "Account");
            }

            // Aynı gün/aynı saatte, aynı berber için randevu var mı kontrol et
            bool isTaken = await _context.Appointments.AnyAsync(a =>
                a.EmployeeId == model.EmployeeId
                && a.Date.Date == model.SelectedDate.Date
                && a.Time == model.SelectedTime);

             if (isTaken)
                {
                    var (prevHour, nextHour) = await GetNearestAvailableHours(model.EmployeeId, model.SelectedDate, model.SelectedTime);

                    TempData["ErrorMessage"] =
                        $"{model.SelectedDate:dd.MM.yyyy} tarihindeki {model.SelectedTime:hh\\:mm} saati bu berber için dolu. " +
                        (prevHour == null && nextHour == null
                            ? "Müsait alternatif saat bulunamadı."
                            : $"Öneri: {(prevHour != null ? $"{prevHour:hh\\:mm}" : "")} {(nextHour != null ? $"ve {nextHour:hh\\:mm}" : "")} saatlerinde randevu alabilirsiniz. Ya da başka bir berber seçebilirsiniz."
                        );

                    // Hata sonrasında da employees listesini doldur:
                    await PopulateEmployees(model);

                    return View(model);
                }


            // Randevu müsait => ekle
            var appointment = new Appointment
            {
                UserId = user.Id, // string
                EmployeeId = model.EmployeeId,
                Date = model.SelectedDate.Date,
                Time = model.SelectedTime,
                // Notes alanı isteğe bağlı => null olabilir
                Notes = string.IsNullOrEmpty(model.Notes) ? "" : model.Notes
            };

            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = "Randevunuz başarıyla oluşturuldu!";
            return RedirectToAction("Create");
        }

        /// <summary>
        /// Verilen tarih ve saat için, bir saat öncesi ve bir saat sonrası (veya benzeri mantık)
        /// hangi saatler müsait, onu bulmak için örnek metod.
        /// </summary>
        /// 
        private async Task PopulateEmployees(AppointmentViewModel model)
        {
            var employees = await _context.Employees.ToListAsync();
            model.Employees = employees.Select(e => new EmployeeSelectViewModel
            {
                EmployeeId = e.Id,
                EmployeeName = e.FullName
            }).ToList();
        }
        private async Task<(TimeSpan?, TimeSpan?)> GetNearestAvailableHours(int employeeId, DateTime date, TimeSpan requestedTime)
        {
            // prevHour: requestedTime'dan geriye doğru en yakın saat
            // nextHour: requestedTime'dan ileriye doğru en yakın saat
            // Mantık: 1 saat önce ve 1 saat sonra bakıyoruz.

            TimeSpan? prev = null;
            TimeSpan? next = null;

            var prevTime = requestedTime - TimeSpan.FromHours(1);
            if (prevTime.Hours >= 9 && prevTime.Hours <= 18) // sabah 9 ile akşam 18 arası
            {
                bool prevTaken = await _context.Appointments.AnyAsync(a =>
                    a.EmployeeId == employeeId
                    && a.Date.Date == date.Date
                    && a.Time == prevTime);

                if (!prevTaken) prev = prevTime;
            }

            var nextTime = requestedTime + TimeSpan.FromHours(1);
            if (nextTime.Hours >= 9 && nextTime.Hours <= 18)
            {
                bool nextTaken = await _context.Appointments.AnyAsync(a =>
                    a.EmployeeId == employeeId
                    && a.Date.Date == date.Date
                    && a.Time == nextTime);

                if (!nextTaken) next = nextTime;
            }

            return (prev, next);
        }
    }
}
