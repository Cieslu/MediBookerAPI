using AutoMapper;
using MediBookerAPI.Data;
using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MediBookerAPI.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IConfiguration _configuration;
        private readonly IMapper _mapper;
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public DoctorService(IConfiguration configuration, ApplicationDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IMapper mapper)
        {
            _configuration = configuration;
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _mapper = mapper;
        }
        public async Task<Boolean> EditDoctor(DoctorPutDTO doctorPutDTO, string doctorId)
        {
            try
            {
                User editDoctor = _mapper.Map<User>(doctorPutDTO);
                User? doctor = await _userManager.FindByIdAsync(doctorPutDTO.Id);
                if(doctor != null)
                {
                    if(doctorPutDTO.File != null)
                    {
                        byte[]? imageData = null;
                        using (var binaryReader = new BinaryReader(doctorPutDTO.File.OpenReadStream()))
                        {
                            imageData = binaryReader.ReadBytes((int)doctorPutDTO.File.Length);
                        }
                        doctor.Image = imageData;
                    }
             
                    doctor.Name = editDoctor.Name;
                    doctor.Surname = editDoctor.Surname;
                    doctor.Specialization = editDoctor.Specialization;
                    doctor.Email = editDoctor.Email;
                    await _userManager.UpdateAsync(doctor);
                    return true;
                }
                return false;
            }
            catch
            {
                throw new Exception("Error while editing a doctor!");
            }
        }

        public async Task<ICollection<ReservationDTO>> LoadReservations(string doctorId)
        {
            try
            {
                List<Reservation> reservations = await _context.Reservations.Where(x => x.Schedule.IdDoctor == doctorId).ToListAsync();
               
                return _mapper.Map<List<ReservationDTO>>(reservations);
            }
            catch
            {
                throw new Exception("Error while lading reservations!");
            }
        }

        public async Task<ICollection<ScheduleDTO>> LoadSchedule(string docotrId)
        {
            try
            {
                List<Schedule> schedule = await _context.Schedules.Where(x => x.IdDoctor == docotrId).ToListAsync();
                return _mapper.Map<List<ScheduleDTO>>(schedule);
            }
            catch
            {
                throw new Exception("Error while lading schedule!");
            }
        }

    }
}
