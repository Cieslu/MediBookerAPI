using Microsoft.AspNetCore.Identity;

namespace MediBookerAPI.ModelsDTO
{
    public class DoctorDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public byte[]? Image { get; set; } = null;
        public IFormFile? File { get; set; } = null;
        public string Specialization { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
