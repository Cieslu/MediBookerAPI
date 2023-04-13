using AutoMapper;
using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MediBookerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase, IDoctorController
    {
        private readonly IMapper _mapper;
        private readonly IDoctorService _doctorService;
        private readonly UserManager<User> _userManager;

        public DoctorController(IMapper mapper, IDoctorService doctorService, UserManager<User> userManager)
        {
            _mapper = mapper;
            _doctorService = doctorService;
            _userManager = userManager;
        }

        [HttpGet("LoadDoctorReservation/{id}")]
        public async Task<ActionResult<ICollection<ReservationDTO>>> GetReservation(string id)
        {
            try
            {
                if (id == null) 
                {
                    return NotFound();
                }
                ICollection<ReservationDTO> reservations = await _doctorService.LoadReservations(id);
                return Ok(reservations);
            }
            catch
            {
                throw new Exception("Error while loading reservations!");
            }
        }

        [HttpGet("LoadDoctorSchedule/{id}")]
        public async Task<ActionResult<ICollection<ScheduleDTO>>> GetSchedule(string id)
        {
            try
            {
                if(id == null)
                {
                    return NotFound();
                }
                ICollection<ScheduleDTO> schedule = await _doctorService.LoadSchedule(id);
                return Ok(schedule);
            }
            catch
            {
                throw new Exception("Error while loading schedule!");
            }
        }

        [HttpPut("EditDoctor/{id}")]
        
        public async Task<ActionResult> PutDoctor([FromForm] DoctorPutDTO doctorPutDTO, string id)
        {
            try
            {
                if (doctorPutDTO == null || doctorPutDTO.Id != id)
                {
                    return NotFound();
                }
                Boolean result = await _doctorService.EditDoctor(doctorPutDTO, id);

                return result ? Ok() : NotFound();

            }
            catch
            {
                throw new Exception("Error while editing a doctor!");
            }
        }


        [HttpGet("GetImage/{id}")]
        public IActionResult GetImage(string id)
        {
            var user = _userManager.FindByIdAsync(id).Result;

            if (user == null || user.Image == null)
            {
                return NotFound();
            }

            return File(user.Image, "image/jpeg");
        }
    }
}
