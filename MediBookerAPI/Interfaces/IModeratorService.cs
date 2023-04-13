using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;

namespace MediBookerAPI.Interfaces
{
    public interface IModeratorService
    {
        public Task<DoctorDTO> PostDoctor(User doctor, string password);
        public Task<WorkerDTO> PostWorker(User worker, string password);
        public Task<ICollection<DoctorDTO>> GetDoctors();
        public Task<DoctorDTO?> GetDoctor(string id);
        public Task<ICollection<WorkerDTO>> GetWorkers();
        public Task<WorkerDTO?> GetWorker(string id);
        public Task<Boolean> EditDoctor(string id, DoctorPutDTO doctorPutDTO);
        public Task<Boolean> EditWorker(string id, WorkerPutDTO workerPutDTO);
        public Task<Boolean> DeleteDoctor(string id);
        public Task<Boolean> DeleteWorker(string id);
        public Task<Boolean> EditModerator(WorkerPutDTO moderatorPutDTO, string moderatorId);
        public string GenerateRandomPassword();
    }
}
