namespace KuaforApp.Models.ViewModels
{
    public class AppointmentViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = "";
        public string? SalonName { get; set; }
        public int EmployeeId { get; set; }

        public List<EmployeeSelectViewModel> Employees { get; set; } = new List<EmployeeSelectViewModel>();
        public string? Notes { get; set; } // Notlar özelliği burada

        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public TimeSpan SelectedTime { get; set; } = new TimeSpan(10, 0, 0);
    }

    public class EmployeeSelectViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
    }
}
