namespace KuaforApp.Models.ViewModels
{
    public class BarberDetailsViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = "";
        public List<ServiceSelectViewModel> Services { get; set; } = new List<ServiceSelectViewModel>();
        public DateTime SelectedDate { get; set; } = DateTime.Today;
        public TimeSpan SelectedTime { get; set; } = new TimeSpan(10, 0, 0);
        public string Notes { get; set; } = "";
    }
}
