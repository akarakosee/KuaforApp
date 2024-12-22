using KuaforApp.Models;
using Microsoft.EntityFrameworkCore;

namespace KuaforApp.Data
{
    public static class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using var scope = serviceProvider.CreateScope();
            var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();

            if (!context.Salons.Any())
            {
                var salon = new Salon
                {
                    Name = "Güzellik ve Kuaför Salonu",
                    Address = "Merkez, Sakarya",
                    OpeningHour = new TimeSpan(9, 0, 0),
                    ClosingHour = new TimeSpan(19, 0, 0),
                };
                context.Salons.Add(salon);
                context.SaveChanges();

                // Servisler
                var s1 = new Service { Name = "Kısa Saç Kesimi", SalonId = salon.Id, DurationInMinutes = 30, Price = 50 };
                var s2 = new Service { Name = "Uzun Saç Kesimi", SalonId = salon.Id, DurationInMinutes = 45, Price = 80 };
                var s3 = new Service { Name = "Fön", SalonId = salon.Id, DurationInMinutes = 20, Price = 30 };
                var s4 = new Service { Name = "Boya", SalonId = salon.Id, DurationInMinutes = 60, Price = 150 };
                var s5 = new Service { Name = "Kil Maskesi", SalonId = salon.Id, DurationInMinutes = 25, Price = 40 };
                var s6 = new Service { Name = "Yüz Ağdası", SalonId = salon.Id, DurationInMinutes = 15, Price = 20 };

                context.Services.AddRange(s1, s2, s3, s4, s5, s6);
                context.SaveChanges();

                var ahmet = new Employee
                {
                    SalonId = salon.Id,
                    FullName = "Ahmet Berber",
                    StartAvailability = new TimeSpan(9, 0, 0),
                    EndAvailability = new TimeSpan(18, 0, 0)
                };
                var serhat = new Employee
                {
                    SalonId = salon.Id,
                    FullName = "Serhat Berber",
                    StartAvailability = new TimeSpan(9, 0, 0),
                    EndAvailability = new TimeSpan(18, 0, 0)
                };
                var ali = new Employee
                {
                    SalonId = salon.Id,
                    FullName = "Ali Berber",
                    StartAvailability = new TimeSpan(9, 0, 0),
                    EndAvailability = new TimeSpan(18, 0, 0)
                };
                context.Employees.AddRange(ahmet, serhat, ali);
                context.SaveChanges();

                // Ahmet: Kısa Saç, Fön, Boya
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ahmet.Id, ServiceId = s1.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ahmet.Id, ServiceId = s3.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ahmet.Id, ServiceId = s4.Id });

                // Serhat: Kısa Saç, Uzun Saç, Kil Maskesi, Yüz Ağdası
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = serhat.Id, ServiceId = s1.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = serhat.Id, ServiceId = s2.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = serhat.Id, ServiceId = s5.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = serhat.Id, ServiceId = s6.Id });

                // Ali: Hepsi
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ali.Id, ServiceId = s1.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ali.Id, ServiceId = s2.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ali.Id, ServiceId = s3.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ali.Id, ServiceId = s4.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ali.Id, ServiceId = s5.Id });
                context.EmployeeServices.Add(new EmployeeService { EmployeeId = ali.Id, ServiceId = s6.Id });

                context.SaveChanges();
            }
        }
    }
}
