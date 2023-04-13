using MediBookerAPI.ModelsDTO;

namespace MediBookerAPI.Interfaces
{
    public interface IDoctorService
    {
        public Task<ICollection<ReservationDTO>> LoadReservations(string doctorId);
        public Task<ICollection<ScheduleDTO>> LoadSchedule(string docotrId);
        public Task<Boolean> EditDoctor(DoctorPutDTO doctorPutDTO, string doctorId);
    }
}
