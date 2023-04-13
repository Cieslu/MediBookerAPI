namespace MediBookerAPI.ModelsDTO
{
    public class DoctorPutDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public IFormFile? File { get; set; } = null;
        public string Specialization { get; set; } = "";
        public string Email { get; set; } = "";
    }
}
