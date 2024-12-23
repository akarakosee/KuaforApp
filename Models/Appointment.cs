namespace KuaforApp.Models
{
    public enum AppointmentStatus
    {
        Pending,
        Confirmed,
        Cancelled
    }

    public class Appointment
    {
        public int Id { get; set; }
        public int EmployeeId { get; set; }
        public Employee Employee { get; set; } = null!;
        public string UserId { get; set; } = ""; // string olarak değiştirildi
        public ApplicationUser User { get; set; } = null!;
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; } // Yeni alan
        public string Notes { get; set; } = string.Empty;
    }
    public class AppointmentService
    {
        public int AppointmentId { get; set; }
        public Appointment? Appointment { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
    }
}
