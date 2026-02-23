using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.Entities;
using Microsoft.EntityFrameworkCore;
public class PublicHolidaySeeder : IDataSeeder
{
    public int Order => 4;
    private readonly AppDbContext _context;

    public PublicHolidaySeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.PublicHolidays.AnyAsync())
            return;

        var holidays = new List<PublicHoliday>
        {
            new PublicHoliday
            {
                HolidayDate = new DateTime(2026, 1, 1),
                IsDeleted = false
            },
            new PublicHoliday
            {
                HolidayDate = new DateTime(2026, 4, 23),
                IsDeleted = false
            },
            new PublicHoliday
            {
                HolidayDate = new DateTime(2026, 5, 1),
                IsDeleted = false
            }
        };

        await _context.PublicHolidays.AddRangeAsync(holidays);
        await _context.SaveChangesAsync();
    }
}
