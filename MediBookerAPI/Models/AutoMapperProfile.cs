using AutoMapper;
using MediBookerAPI.ModelsDTO;

namespace MediBookerAPI.Models
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile() 
        {
            CreateMap<User, DoctorDTO>();
            CreateMap<DoctorPutDTO, User>();
            CreateMap<DoctorDTO, User>();
            CreateMap<User, WorkerDTO>();
            CreateMap<WorkerPutDTO, User>();
            CreateMap<WorkerDTO, User>();
            CreateMap<Schedule, ScheduleDTO>();
            CreateMap<ScheduleDTO, Schedule>();
            CreateMap<Reservation, ReservationDTO>();
            CreateMap<ReservationDTO, Reservation>();
        }
    }
}
