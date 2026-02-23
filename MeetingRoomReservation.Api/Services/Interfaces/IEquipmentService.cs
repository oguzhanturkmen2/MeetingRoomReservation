using MeetingRoomReservation.Api.Entities;

namespace MeetingRoomReservation.Api.Services.Interfaces
{
    public interface IEquipmentService
    {
        Task<List<Equipment>> GetAllAsync();
        Task<Equipment?> GetByIdAsync(int id);
        Task CreateAsync(string name, string? specification);
        Task UpdateAsync(int id, string name, string? specification);
        Task DeleteAsync(int id);
    }

}
