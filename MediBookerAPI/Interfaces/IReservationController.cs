using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Mvc;

namespace MediBookerAPI.Interfaces
{
    public interface IReservationController
    {
        [HttpPost("AddReservation")]
        public Task<ActionResult<ReservationDTO?>> AddReservation(ReservationDTO reservationDTO);

        [HttpGet("LoadReservations")]
        public Task<ActionResult<ICollection<ReservationDTO>>> LoadReservation();

        [HttpDelete("DeleteReservation/{id}")]
        public Task<ActionResult> DeleteReservation(int id);

        [HttpGet("LoadReservations/{id}")]
        public Task<ActionResult<ICollection<ReservationDTO>>> LoadReservation(string id);
    }
}
