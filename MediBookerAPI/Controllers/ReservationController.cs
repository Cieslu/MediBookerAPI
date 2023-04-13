using MediBookerAPI.Interfaces;
using MediBookerAPI.ModelsDTO;
using MediBookerAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MediBookerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase, IReservationController
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("AddReservation")]
        public async Task<ActionResult<ReservationDTO?>> AddReservation(ReservationDTO reservationDTO)
        {
            try
            {
                if(reservationDTO == null)
                {
                    return NotFound();
                }
                ReservationDTO? newReservation = await _reservationService.PostReservation(reservationDTO);
                if(newReservation != null)
                {
                    return Ok(newReservation);
                }
                else
                {
                    return newReservation;
                }
            }
            catch
            {
                throw new Exception("Error while adding a reservation!");
            }
        }

        [HttpDelete("DeleteReservation/{id}")]
        public async Task<ActionResult> DeleteReservation(int id)
        {
            try
            {
                ReservationDTO? reservation = await _reservationService.DeleteReservation(id);
                if (reservation == null)
                {
                    return NotFound();
                }
                return NoContent();
            }
            catch
            {
                throw new Exception("Error while deleting a reservation!");
            }
        }

        [HttpGet("LoadReservations")]
        public async Task<ActionResult<ICollection<ReservationDTO>>> LoadReservation()
        {
            try
            {
                ICollection<ReservationDTO> reservations = await _reservationService.GetReservations();

                return Ok(reservations);
            }
            catch
            {
                throw new Exception("Error while loading reservations!");
            }
        }

        [HttpGet("LoadReservation/{id}")]
        public async Task<ActionResult<ICollection<ReservationDTO>>> LoadReservation(string id)
        {
            try
            {
                ICollection<ReservationDTO> reservations = await _reservationService.GetReservations(id);

                return Ok(reservations);
            }
            catch
            {
                throw new Exception("Error while loading reservations!");
            }
        }
    }
}
