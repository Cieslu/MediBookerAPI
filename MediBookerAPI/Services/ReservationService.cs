using AutoMapper;
using MediBookerAPI.Data;
using MediBookerAPI.Interfaces;
using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace MediBookerAPI.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public ReservationService(ApplicationDbContext context, IMapper mapper, UserManager<User> userManager, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public async Task<ReservationDTO?> PostReservation(ReservationDTO reservationDTO)
        {
            try 
            {
                Reservation resrvation = _mapper.Map<Reservation>(reservationDTO);
                Boolean booked = await _context.Reservations.AnyAsync(x => x.ScheduleId == reservationDTO.ScheduleId);
                if (!booked)
                {
                    _context.Reservations.Add(resrvation);
                    await _context.SaveChangesAsync();
                    return _mapper.Map<ReservationDTO>(resrvation);
                }
                return null;
            }
            catch
            {
                throw new Exception("Error while adding a reservation!");
            }
        }

        public async Task<ICollection<ReservationDTO>> GetReservations()
        {
            try
            {
                IList<Reservation> reservations = await _context.Reservations.ToListAsync();
                IList<ReservationDTO> reservationsDTO = _mapper.Map<IList<ReservationDTO>>(reservations);

                return reservationsDTO;
            }
            catch
            {
                throw new Exception("Error while loading reservations!");
            }
        }

        public async Task<ICollection<ReservationDTO>> GetReservations(string id)
        {
            try
            {
                IList<Reservation> reservations = await _context.Reservations.Where(x => x.Schedule.IdDoctor == id).ToListAsync();
                IList<ReservationDTO> reservationsDTO = _mapper.Map<IList<ReservationDTO>>(reservations);

                return reservationsDTO;
            }
            catch
            {
                throw new Exception("Error while loading reservations!");
            }
        }

        public async Task<ReservationDTO?> DeleteReservation(int id)
        {
            try
            {
                Reservation? reservation = await _context.Reservations.FirstOrDefaultAsync(x => x.Id == id);

                if (reservation != null)
                {
                    _context.Reservations.Remove(reservation);
                    await _context.SaveChangesAsync();

                }
                return _mapper.Map<ReservationDTO>(reservation);
            }
            catch
            {
                throw new Exception("Error while deleting a reservtion!");
            }
        }
    }
}
