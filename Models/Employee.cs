namespace KuaforApp.Models
{
    public class Employee
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }
        public string FullName { get; set; } = "";
        public TimeSpan StartAvailability { get; set; }
        public TimeSpan EndAvailability { get; set; }
        public ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();
        public List<Appointment>? Appointments { get; set; }
    }
}
