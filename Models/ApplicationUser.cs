using Microsoft.AspNetCore.Identity;

namespace KuaforApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = "";
        // PhoneNumber özelliği IdentityUser'dan geliyor.
    }
}
