namespace KuaforApp.Models
{
    public class Salon
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Güzellik ve Kuaför Salonu";
        public string Address { get; set; } = "Merkez, Sakarya";
        public TimeSpan OpeningHour { get; set; } = new TimeSpan(9, 0, 0);
        public TimeSpan ClosingHour { get; set; } = new TimeSpan(19, 0, 0);
        public List<Service> Services { get; set; } = new();
        public List<Employee> Employees { get; set; } = new();
    }
}
