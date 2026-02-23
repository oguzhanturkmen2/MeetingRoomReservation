using MeetingRoomReservation.Api.DTOs;

namespace MeetingRoomReservation.Api.Services.Interfaces
{
    public interface IRoomService
    {
        Task<List<RoomDto>> GetAllAsync();
        Task<RoomDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateRoomDto dto);
        Task UpdateAsync(int id, CreateRoomDto dto);
        Task DeleteAsync(int id);
    }
}