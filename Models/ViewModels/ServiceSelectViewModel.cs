namespace KuaforApp.Models.ViewModels
{
    public class ServiceSelectViewModel
    {
        public int ServiceId { get; set; }
        public string ServiceName { get; set; } = "";
        public bool Selected { get; set; } = false;
    }
}
