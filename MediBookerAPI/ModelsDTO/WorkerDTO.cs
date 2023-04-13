namespace MediBookerAPI.ModelsDTO
{
    public class WorkerDTO
    {
        public string Id { get; set; } = "";
        public string Name { get; set; } = "";
        public string Surname { get; set; } = "";
        public byte[]? Image { get; set; } = null;
        public IFormFile? File { get; set; } = null;
        public string Email { get; set; } = "";
    }
}
