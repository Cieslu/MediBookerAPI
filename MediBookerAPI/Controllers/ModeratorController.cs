using MediBookerAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using AutoMapper;
using MediBookerAPI.ModelsDTO;
using MediBookerAPI.Models;
using Microsoft.AspNetCore.Identity;
using MediBookerAPI.Services;
using MediBookerAPI.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace MediBookerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Moderator")]
    public class ModeratorController : ControllerBase, IModeratorController
    {
        private readonly IMapper _mapper;
        private readonly IModeratorService _moderatorService;
        private readonly UserManager<User> _userManager;

        public ModeratorController(IMapper mapper, IModeratorService userService, UserManager<User> userManager)
        {
            _mapper = mapper;
            _moderatorService = userService;
            _userManager = userManager;
        }

        [HttpPost("CreateDoctor")]
        public async Task<ActionResult<DoctorDTO>> CreateDoctor(RegisterUserDTO registerUserDTO)
        {
            try
            {
                string login = registerUserDTO.Email;
                string password = _moderatorService.GenerateRandomPassword();

                User doctor = new User();
                doctor.Email = login;
                doctor.UserName= login;
                DoctorDTO newDoctor = await _moderatorService.PostDoctor(doctor, password);
               
                return Ok(newDoctor);
            }
            catch 
            {
                throw new Exception("Error while creating a new user!");
            }
        }

        [HttpPost("CreateWorker")]
        public async Task<ActionResult<WorkerDTO>> CreateWorker(RegisterUserDTO registerUserDTO)
        {
            try
            {
                string login = registerUserDTO.Email;
                string password = _moderatorService.GenerateRandomPassword();

                User worker = new User();
                worker.Email = login;
                worker.UserName = login;
                WorkerDTO newWorker = await _moderatorService.PostWorker(worker, password);

                return Ok(newWorker);
            }
            catch 
            {
                throw new Exception("Error while creating a new user!");
            }
        }

        [HttpGet("LoadDoctors")]
        public async Task<ActionResult<ICollection<DoctorDTO>>> LoadDoctors()
        {
            try
            {
                ICollection<DoctorDTO> doctors = await _moderatorService.GetDoctors();
                return Ok(doctors);
            }
            catch
            {
                throw new Exception("Error while loading a doctors!");
            }
        }

        [HttpGet("LoadDoctor/{id}")]
        public async Task<ActionResult<DoctorDTO>> LoadDoctor(string id)
        {
            try
            {
                DoctorDTO? doctor = await _moderatorService.GetDoctor(id);
                return Ok(doctor);
            }
            catch
            {
                throw new Exception("Error while loading a doctor!");
            }
        }

        [HttpGet("LoadWorkers")]
        public async Task<ActionResult<ICollection<WorkerDTO>>> LoadWorkers()
        {
            try
            {
                ICollection<WorkerDTO> workers = await _moderatorService.GetWorkers();
                return Ok(workers);
            }
            catch
            {
                throw new Exception("Error while loading a workers!");
            }
        }

        [HttpGet("LoadWorker/{id}")]
        public async Task<ActionResult<WorkerDTO>> LoadWorker(string id)
        {
            try
            {
                WorkerDTO? worker = await _moderatorService.GetWorker(id);
                return Ok(worker);
            }
            catch
            {
                throw new Exception("Error while loading a worker!");
            }
        }

        [HttpPut("EditDoctor/{id}")]
        public async Task<ActionResult> EditDoctor(string id, [FromForm]  DoctorPutDTO doctorPutDTO)
        {
            try
            {
                if (doctorPutDTO != null && doctorPutDTO.Id == id)
                {
                    Boolean result = await _moderatorService.EditDoctor(id, doctorPutDTO);
             
                    return result ? Ok() : NotFound();
                }
                             
                return NotFound();          
            }
            catch
            {
                throw new Exception("Error while editing a doctor!");
            }
        }

        [HttpPut("EditWorker/{id}")]
        public async Task<ActionResult> EditWorker(string id, [FromForm]  WorkerPutDTO workerPutDTO)
        {
            try
            {
                if (workerPutDTO != null && workerPutDTO.Id == id)
                {
                    Boolean result = await _moderatorService.EditWorker(id, workerPutDTO);

                    return result ? Ok() : NotFound();
                }

                return NotFound();
            }
            catch
            {
                throw new Exception("Error while editing a worker!");
            }
        }

        [HttpDelete("DeleteDoctor/{id}")]
        public async Task<ActionResult> DeleteDoctor(string id)
        {
            try
            {
                Boolean result = await _moderatorService.DeleteDoctor(id);
              
                return result ? NoContent() : BadRequest();
            }
            catch
            {
                throw new Exception("Error while deleting a doctor!");
            }
        }

        [HttpDelete("DeleteWorker/{id}")]
        public async Task<ActionResult> DeleteWorker(string id)
        {
            try
            {
                Boolean result = await _moderatorService.DeleteWorker(id);

                return result ? NoContent() : BadRequest();
            }
            catch
            {
                throw new Exception("Error while deleting a worker!");
            }
        }

        [HttpPut("EditModerator/{id}")]
        public async Task<ActionResult> PutModerator([FromForm] WorkerPutDTO moderatorPutDTO, string id)
        {
            try
            {
                if (moderatorPutDTO == null || moderatorPutDTO.Id != id)
                {
                    return NotFound();
                }
                Boolean result = await _moderatorService.EditModerator(moderatorPutDTO, id);

                return result ? Ok() : NotFound();

            }
            catch
            {
                throw new Exception("Error while editing a doctor!");
            }
        }
    }
}
