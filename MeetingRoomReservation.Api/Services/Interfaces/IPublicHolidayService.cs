namespace MeetingRoomReservation.Api.Services.Interfaces
{
    public interface IPublicHolidayService
    {
        Task<List<DateTime>> GetAllAsync();
        Task<DateTime?> GetByIdAsync(int id);
        Task CreateAsync(DateTime date);
        Task UpdateAsync(int id, DateTime date);
        Task DeleteAsync(int id);

        Task<bool> IsPublicHolidayAsync(DateTime date);
    }
}
