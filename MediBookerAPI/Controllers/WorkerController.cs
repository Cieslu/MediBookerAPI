using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;
using MediBookerAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace MediBookerAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Worker")]
    public class WorkerController : ControllerBase, IWorkerController
    {
        private readonly IWorkerService _workerService;

        public WorkerController(IWorkerService userService)
        {
            _workerService = userService;
        }

        [HttpPost("CreateDay")]
        public async Task<ActionResult<ScheduleDTO>> CreateDay(ScheduleDTO scheduleDTO)
        {
            try
            {
                if(scheduleDTO == null)
                {
                    return NotFound();
                }

                ScheduleDTO scheduleDayDTO = await _workerService.PostDay(scheduleDTO);

                return Ok(scheduleDayDTO);

            }
            catch
            {
                throw new Exception("Error while creating a day in schedule!");
            }
        }

        [HttpPut("EditeDay/{id}")]
        public async Task<ActionResult> EditDay(int id, ScheduleDTO scheduleDTO)
        {
            try
            {
                if (scheduleDTO != null && scheduleDTO.Id == id)
                {
                    Boolean result = await _workerService.EditDay(id, scheduleDTO);

                    return result ? Ok() : NotFound();
                }
                return NotFound();
            }
            catch
            {
                throw new Exception("Error while editing a day in schedule!");
            }
        }

        [HttpDelete("DeleteDay/{id}")]
        public async Task<ActionResult> DeleteDay(int id)
        {
            try
            {
                Boolean result = await _workerService.DeleteDay(id);
              
                return result ? NoContent() : BadRequest();
            }
            catch
            {
                throw new Exception("Error while deleting a day in schedule!");
            }
        }

        [HttpGet("LoadDay")]
        public async Task<ActionResult<ICollection<ScheduleDTO>>> LoadDays()
        {
            try
            {
                ICollection<ScheduleDTO> days = await _workerService.GetDays();

                return Ok(days);
            }
            catch
            {
                throw new Exception("Error while loading a days!");
            }
        }

        [HttpGet("LoadDay/{id}")]
        public async Task<ActionResult<ScheduleDTO>> LoadDay(int id)
        {
            try
            {
                ScheduleDTO? day = await _workerService.GetDay(id);
                return Ok(day);
            }
            catch
            {
                throw new Exception("Error while loading a worker!");
            }
        }

        [HttpPut("EditWorker/{id}")]
        public async Task<ActionResult> PutWorker([FromForm] WorkerPutDTO workerPutDTO, string id)
        {
            try
            {
                if (workerPutDTO == null || workerPutDTO.Id != id)
                {
                    return NotFound();
                }
                Boolean result = await _workerService.EditWorker(workerPutDTO, id);

                return result ? Ok() : NotFound();

            }
            catch
            {
                throw new Exception("Error while editing a doctor!");
            }
        }
    }
}
