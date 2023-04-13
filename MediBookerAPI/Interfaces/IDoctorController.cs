using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Mvc;

namespace MediBookerAPI.Interfaces
{
    public interface IDoctorController
    {
        [HttpGet("LoadDoctorReservation/{id}")]
        public Task<ActionResult<ICollection<ReservationDTO>>> GetReservation(string id);

        [HttpGet("LoadDoctorSchedule/{id}")]
        public Task<ActionResult<ICollection<ScheduleDTO>>> GetSchedule(string id);

        [HttpPut("EditDoctor/{id}")]
        public Task<ActionResult> PutDoctor([FromForm] DoctorPutDTO doctorPutDTO, string id);

    }
}
