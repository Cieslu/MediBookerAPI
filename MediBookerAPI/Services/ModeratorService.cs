using AutoMapper;
using MediBookerAPI.Data;
using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;
using NuGet.Protocol;
using PasswordGenerator;
using System.Numerics;
using System.Security.Cryptography;
using System.Text;

namespace MediBookerAPI.Services
{
    public class ModeratorService : IModeratorService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ModeratorService(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IEmailSender emailSender)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
            _emailSender = emailSender;
        }

        public async Task<DoctorDTO> PostDoctor(User doctor, string password)
        {
            try
            {
                IdentityResult resultUser =  await _userManager.CreateAsync(doctor, password);
                IdentityResult resultRole = await _userManager.AddToRoleAsync(doctor, "Doctor");

                if (resultUser.Succeeded == true && resultRole.Succeeded == true)
                {
                    string message = $"<p>Login: {doctor.Email}</p><p>Hasło: {password}</p>";

                    await _emailSender.SendEmailAsync(doctor.Email!, "Register", message);
                }

                return _mapper.Map<DoctorDTO>(doctor);
            }
            catch
            {
                throw new Exception("Error while creating a new user!");
            }
        }

        public async Task<WorkerDTO> PostWorker(User worker, string password)
        {
            try
            {
                IdentityResult resultUser =  await _userManager.CreateAsync(worker, password);
                IdentityResult resultRole = await _userManager.AddToRoleAsync(worker, "Worker");

                if (resultUser.Succeeded == true && resultRole.Succeeded == true)
                {
                    string message = $"<p>Login: {worker.Email}</p><p>Hasło: {password}</p>";

                    await _emailSender.SendEmailAsync(worker.Email!, "Register", message);
                }

                return _mapper.Map<WorkerDTO>(worker);

            }
            catch
            {
                throw new Exception("Error while creating a new user!");
            }
        }

        public async Task<ICollection<DoctorDTO>> GetDoctors()
        {
            try
            {
                IList<User> doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                return _mapper.Map<ICollection<DoctorDTO>>(doctors);
            }
            catch
            {
                throw new Exception("Error while loading a list of doctors!");
            }
        }

        public async Task<DoctorDTO?> GetDoctor(string id)
        {
            try
            {
                IList<User> doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                User? doctor = doctors.Where(x => x.Id == id).FirstOrDefault();

                return _mapper.Map<DoctorDTO>(doctor); ;
            }
            catch
            {
                throw new Exception("Error while loading a doctor!");
            }
        }

        public async Task<ICollection<WorkerDTO>> GetWorkers()
        {
            try
            {
                IList<User> workers = await _userManager.GetUsersInRoleAsync("Worker");
                return _mapper.Map<ICollection<WorkerDTO>>(workers);
            }
            catch
            {
                throw new Exception("Error while loading a list of workers!");
            }
        }

        public async Task<WorkerDTO?> GetWorker(string id)
        {
            try
            {
                IList<User> workers = await _userManager.GetUsersInRoleAsync("Worker");
                User? worker = workers.Where(x => x.Id == id).FirstOrDefault();

                return _mapper.Map<WorkerDTO>(worker); ;
            }
            catch
            {
                throw new Exception("Error while loading a worker!");
            }
        }

        public async Task<Boolean> EditDoctor(string id, DoctorPutDTO doctorPutDTO)
        {
            try
            {  
                IList<User> doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                User? doctor = doctors.Where(x => x.Id == id).FirstOrDefault();

                if(doctor != null)
                {
                    if (doctorPutDTO.File != null)
                    {
                        byte[]? imageData = null;
                        using (var binaryReader = new BinaryReader(doctorPutDTO.File.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)doctorPutDTO.File.Length);
                        }
                        doctor.Image = imageData;
                    }

                    doctor.Id = doctorPutDTO.Id;
                    doctor.UserName = doctorPutDTO.Email;
                    doctor.NormalizedUserName = doctorPutDTO.Email.ToUpper();
                    doctor.NormalizedEmail = doctorPutDTO.Email.ToUpper();
                    doctor.Name = doctorPutDTO.Name;
                    doctor.Surname = doctorPutDTO.Surname;
                    doctor.Specialization = doctorPutDTO.Specialization;
                    doctor.Email = doctorPutDTO.Email;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                throw new Exception("Error while updating a user!");
            }
        }

        public async Task<Boolean> EditWorker(string id, WorkerPutDTO workerPutDTO)
        {
            try
            {
                IList<User> workers = await _userManager.GetUsersInRoleAsync("Worker");
                User? worker = workers.Where(x => x.Id == id).FirstOrDefault();

                if (worker != null)
                {
                    if (workerPutDTO.File != null)
                    {
                        byte[]? imageData = null;
                        using (var binaryReader = new BinaryReader(workerPutDTO.File.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)workerPutDTO.File.Length);
                        }
                        worker.Image = imageData;
                    }

                    worker.Id = workerPutDTO.Id;
                    worker.UserName = workerPutDTO.Email;
                    worker.NormalizedUserName = workerPutDTO.Email.ToUpper();
                    worker.NormalizedEmail = workerPutDTO.Email.ToUpper();
                    worker.Name = workerPutDTO.Name;
                    worker.Surname = workerPutDTO.Surname;
                    worker.Email = workerPutDTO.Email;

                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch
            {
                throw new Exception("Error while updating a user!");
            }
        }

        public async Task<Boolean> DeleteDoctor(string id)
        {
            try
            {
                IList<User> doctors = await _userManager.GetUsersInRoleAsync("Doctor");
                User? doctor = doctors.Where(x => x.Id == id).FirstOrDefault();

                if (doctor != null)
                {
                    _context.Users.Remove(doctor);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new Exception("Error while updating a user!");
            }
        }

        public async Task<Boolean> DeleteWorker(string id)
        {
            try
            {
                IList<User> workers = await _userManager.GetUsersInRoleAsync("Worker");
                User? worker = workers.Where(x => x.Id == id).FirstOrDefault();

                if (worker != null)
                {
                    _context.Users.Remove(worker);
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch
            {
                throw new Exception("Error while updating a user!");
            }
        }

        public async Task<Boolean> EditModerator(WorkerPutDTO moderatorPutDTO, string moderatorId)
        {
            try
            {
                User editModerator = _mapper.Map<User>(moderatorPutDTO);
                User? moderator = await _userManager.FindByIdAsync(moderatorPutDTO.Id);
                if (moderator != null)
                {
                    if(moderatorPutDTO.File != null)
                    {
                        byte[]? imageData = null;

                        using (var binaryReader = new BinaryReader(moderatorPutDTO.File.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)moderatorPutDTO.File.Length);
                        }
                        moderator.Image = imageData;
                    }

                    moderator.Name = editModerator.Name;
                    moderator.Surname = editModerator.Surname;
                    moderator.Email = editModerator.Email;
                    await _userManager.UpdateAsync(moderator);
                    return true;
                }
                return false;
            }
            catch
            {
                throw new Exception("Error while editing a doctor!");
            }
        }

        public string GenerateRandomPassword()
        {
            var pwd = new Password();
            var password = pwd.Next();
            return password;
        }
    }
}
