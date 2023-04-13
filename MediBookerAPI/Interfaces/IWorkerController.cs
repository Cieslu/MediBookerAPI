using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Mvc;

namespace MediBookerAPI.Interfaces
{
    public interface IWorkerController
    {
        [HttpPost("CreateDay")]
        public Task<ActionResult<ScheduleDTO>> CreateDay(ScheduleDTO scheduleDTO);

        [HttpPut("EditeDay/{id}")]
        public Task<ActionResult> EditDay(int id, ScheduleDTO scheduleDTO);

        [HttpDelete("DeleteDay/{id}")]
        public Task<ActionResult> DeleteDay(int id);

        [HttpGet("LoadDay")]
        public Task<ActionResult<ICollection<ScheduleDTO>>> LoadDays();

        [HttpGet("LoadDay/{id}")]
        public Task<ActionResult<ScheduleDTO>> LoadDay(int id);

        [HttpPut("EditWorker/{id}")]
        public Task<ActionResult> PutWorker([FromForm] WorkerPutDTO workerPutDTO, string id);
    }
}
