using Microsoft.AspNetCore.Identity;

namespace MediBookerAPI.Models
{
    public class User : IdentityUser
    {
        public string? Name { get; set; }
        public string? Surname { get; set; }
        public byte[]? Image { get; set; }
        public string? Specialization { get; set; }
    }
}
