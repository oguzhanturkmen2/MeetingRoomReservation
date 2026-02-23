using MeetingRoomReservation.Api.DTOs;

namespace MeetingRoomReservation.Api.Services.Interfaces
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllAsync();
        Task<UserDto?> GetByIdAsync(int id);
        Task<int> CreateAsync(CreateUpdateUserDto dto);
        Task UpdateAsync(int id, CreateUpdateUserDto dto);
        Task DeleteAsync(int id);
    }

}
