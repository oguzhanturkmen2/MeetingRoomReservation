using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Entities;

namespace MeetingRoomReservation.Api.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateReservationDto dto);
        Task UpdateAsync(int id, CreateReservationDto dto);
        Task DeleteAsync(int id);
    }
}