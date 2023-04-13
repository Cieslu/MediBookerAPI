using AutoMapper;
using MediBookerAPI.Data;
using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Numerics;
using System.Reflection.Metadata;

namespace MediBookerAPI.Services
{
    public class WorkerService : IWorkerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public WorkerService(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ScheduleDTO> PostDay(ScheduleDTO scheduleDTO)
        {
            try
            {
                List<Schedule> schedules = new List<Schedule>();

                Schedule schedule = _mapper.Map<Schedule>(scheduleDTO);

                int totalHours = CalculateHours(schedule);
                string startHour = "";

                for (int i = 0; i < totalHours; i++)
                {
                    Schedule newSchedule = new Schedule();
                    if (i == 0)
                    {
                        newSchedule.HoursFrom = schedule.HoursFrom;
                    }
                    else
                    {
                        newSchedule.HoursFrom = startHour;
                    }

                    if (!newSchedule.HoursFrom.Contains(",30") && !newSchedule.HoursFrom.Contains(",3") && !newSchedule.HoursFrom.Contains(",0") && !newSchedule.HoursFrom.Contains(",00"))
                    {
                        throw new Exception("Incorrect time format specified!");

                    }
                    else if (!newSchedule.HoursFrom.Contains(",30") && !newSchedule.HoursFrom.Contains(",3"))
                    {
                        double convertHour = Convert.ToDouble(newSchedule.HoursFrom);
                        DateTime dateTimeHourFrom = new DateTime(1, 1, 1, Convert.ToInt32(convertHour), 0, 0);
                        DateTime dateTimeHourToWithMinutes = dateTimeHourFrom.AddMinutes(30);

                        newSchedule.HoursFrom = Convert.ToString(Convert.ToInt32(convertHour) + ":" + dateTimeHourFrom.Minute);
                        newSchedule.HoursTo = Convert.ToString(dateTimeHourToWithMinutes.Hour + ":" + dateTimeHourToWithMinutes.Minute);

                        schedules.Add(newSchedule);

                        string hourF = newSchedule.HoursFrom.Substring(0, newSchedule.HoursFrom.IndexOf(":"));
                        string hourT = newSchedule.HoursFrom.Substring(0, newSchedule.HoursTo.IndexOf(":"));

                        DateTime minutes = dateTimeHourFrom.AddMinutes(30);

                        startHour = Convert.ToString(hourF + "," + minutes.Minute);
                    }
                    else
                    {
                        string hour = newSchedule.HoursFrom.Substring(0, newSchedule.HoursFrom.IndexOf(","));

                        DateTime dateTimeHourFrom = new DateTime(1, 1, 1, int.Parse(hour), 0, 0);
                        DateTime dateTimeHourFromWithMinutes = dateTimeHourFrom.AddMinutes(30);
                        DateTime dateTimeHourToWithMinutes = dateTimeHourFromWithMinutes.AddMinutes(30);

                        Schedule xd = new Schedule
                        {
                            HoursFrom = Convert.ToString(dateTimeHourFromWithMinutes.Hour + ":" + dateTimeHourFromWithMinutes.Minute),
                            HoursTo = Convert.ToString(dateTimeHourToWithMinutes.Hour + ":" + dateTimeHourToWithMinutes.Minute),
                            Date = newSchedule.Date,
                            IdDoctor = newSchedule.IdDoctor
                        };

                        schedules.Add(xd);

                        string hourF = xd.HoursFrom.Substring(0, xd.HoursFrom.IndexOf(":"));

                        DateTime minutes = dateTimeHourFrom.AddHours(1);

                        startHour = Convert.ToString(minutes.Hour + ",00");
                    }
                }

                schedules.ForEach(x =>
                {
                    _context.Schedules.Add(x);
                });
                await _context.SaveChangesAsync();


                return scheduleDTO;
            }
            catch 
            {
                throw new Exception("Error while creating a day in schedule!");
            }
        }

        public async Task<Boolean> EditDay(int id, ScheduleDTO scheduleDTO)
        {
            try
            {
                Schedule editDay = _mapper.Map<Schedule>(scheduleDTO);
                Schedule? day = await _context.Schedules.FirstOrDefaultAsync(x => x.Id == id);

                if (day != null)
                {
                    day.Id = editDay.Id;
                    day.IdDoctor = editDay.IdDoctor;
                    day.Date = editDay.Date;
                    day.HoursFrom = editDay.HoursFrom;
                    day.HoursTo = editDay.HoursTo;

                    await _context.SaveChangesAsync(); 
                    return true;
                }

                return false;

            }
            catch
            {
                throw new Exception("Error while editing a day in schedule!");
            }
        }

        public async Task<Boolean> DeleteDay(int id)
        {
            try
            {
                Schedule? day = await _context.Schedules.FirstOrDefaultAsync(x => x.Id == id);

                if (day != null)
                {
                    _context.Schedules.Remove(day);
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

        public async Task<ICollection<ScheduleDTO>> GetDays()
        {
            try
            {
                IList<Schedule> days = await _context.Schedules.ToListAsync();
                IList<ScheduleDTO> daysDTO = _mapper.Map<IList<ScheduleDTO>>(days);

                return daysDTO;
            }
            catch
            {
                throw new Exception("Error while loading a days!");
            }
        }

        public async Task<ScheduleDTO?> GetDay(int id)
        {
            try
            {
                Schedule? day = await _context.Schedules.FirstOrDefaultAsync(x => x.Id == id);

                return _mapper.Map<ScheduleDTO>(day);
            }
            catch
            {
                throw new Exception("Error while loading a day!");
            }
        }

        public async Task<Boolean> EditWorker(WorkerPutDTO workerPutDTO, string workerId)
        {
            try
            {
                User editWorker = _mapper.Map<User>(workerPutDTO);
                User? worker = await _userManager.FindByIdAsync(workerPutDTO.Id);
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

                    worker.Name = editWorker.Name;
                    worker.Surname = editWorker.Surname;
                    worker.Email = editWorker.Email;
                    await _userManager.UpdateAsync(worker);
                    return true;
                }
                return false;
            }
            catch
            {
                throw new Exception("Error while editing a doctor!");
            }
        }

        public int CalculateHours(Schedule schedule)
        {
            double hourFrom = Convert.ToDouble(schedule.HoursFrom);
            double hourTo = Convert.ToDouble(schedule.HoursTo);
            double mathematicalDifference = hourTo - hourFrom;

            return (int)(mathematicalDifference / 0.5);
        }
    }
}
