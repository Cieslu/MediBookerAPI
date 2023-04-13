using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Mvc;

namespace MediBookerAPI.Interfaces
{
    public interface IModeratorController
    {
        [HttpPost("CreateDoctor")]
        public Task<ActionResult<DoctorDTO>> CreateDoctor(RegisterUserDTO registerUserDTO);

        [HttpPost("CreateWorker")]
        public Task<ActionResult<WorkerDTO>> CreateWorker(RegisterUserDTO registerUserDTO);

        [HttpGet("LoadDoctors")]
        public Task<ActionResult<ICollection<DoctorDTO>>> LoadDoctors();

        [HttpGet("LoadDoctor/{id}")]
        public Task<ActionResult<DoctorDTO>> LoadDoctor(string id);

        [HttpGet("LoadWorkers")]
        public Task<ActionResult<ICollection<WorkerDTO>>> LoadWorkers();

        [HttpGet("LoadWorker/{id}")]
        public Task<ActionResult<WorkerDTO>> LoadWorker(string id);

        [HttpPut("EditDoctor/{id}")]
        public Task<ActionResult> EditDoctor(string id, DoctorPutDTO doctorPutDTO);

        [HttpPut("EditWorker/{id}")]
        public Task<ActionResult> EditWorker(string id, WorkerPutDTO workerPutDTO);

        [HttpPut("EditModerator/{id}")]
        public Task<ActionResult> PutModerator([FromForm] WorkerPutDTO moderatorPutDTO, string id);

        [HttpDelete("DeleteDoctor/{id}")]
        public Task<ActionResult> DeleteDoctor(string id);

        [HttpPut("DeleteWorker/{id}")]
        public Task<ActionResult> DeleteWorker(string id);
    }
}
