﻿namespace KuaforApp.Models
{
    public class Service
    {
        public int Id { get; set; }
        public int SalonId { get; set; }
        public Salon? Salon { get; set; }

        public string Name { get; set; } = "";
        public int DurationInMinutes { get; set; }
        public decimal Price { get; set; }
        public ICollection<EmployeeService> EmployeeServices { get; set; } = new List<EmployeeService>();
    }
}
