using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Entities;

namespace MeetingRoomReservation.Api.Services.Interfaces
{
    public interface IReservationService
    {
        Task<List<Reservation>> GetAllAsync();
        Task<Reservation?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateUpdateReservationDto dto);
        Task UpdateAsync(int id, CreateUpdateReservationDto dto);
        Task DeleteAsync(int id);
        Task<bool> HasConflictAsync(int roomId, DateTime start, DateTime end);

    }
}