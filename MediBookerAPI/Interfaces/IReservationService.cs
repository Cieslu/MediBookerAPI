using MediBookerAPI.Models;
using MediBookerAPI.ModelsDTO;

namespace MediBookerAPI.Interfaces
{
    public interface IReservationService
    {
        public Task<ReservationDTO?> PostReservation(ReservationDTO reservationDTO);
        public Task<ICollection<ReservationDTO>> GetReservations();
        public Task<ICollection<ReservationDTO>> GetReservations(string id);
        public Task<ReservationDTO?> DeleteReservation(int id);
    }
}
