using MeetingRoomReservation.Api.DTOs;

namespace MeetingRoomReservation.Api.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllAsync();
        Task<RoomDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateUpdateRoomDto dto);
        Task UpdateAsync(int id, CreateUpdateRoomDto dto);
        Task DeleteAsync(int id);
    }
}