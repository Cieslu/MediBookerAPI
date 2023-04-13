using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;

namespace MediBookerAPI.Interfaces
{
    public interface IWorkerService
    {
        public Task<ScheduleDTO> PostDay(ScheduleDTO scheduleDTO);
        public Task<Boolean> EditDay(int id, ScheduleDTO scheduleDTO);
        public Task<Boolean> DeleteDay(int id);
        public Task<ICollection<ScheduleDTO>> GetDays();
        public Task<ScheduleDTO?> GetDay(int id);
        public Task<Boolean> EditWorker(WorkerPutDTO workerPutDTO, string workerId);
        public int CalculateHours(Schedule schedule);
    }
}
