using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.Entities;
using Microsoft.EntityFrameworkCore;

public class RoomSeeder : IDataSeeder
{
    public int Order => 2;
    private readonly AppDbContext _context;

    public RoomSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Rooms.AnyAsync())
            return;

        var rooms = new List<Room>
        {
            new Room { Name = "Toplantı Odası A", Capacity = 5, IsDeleted = false },
            new Room { Name = "Toplantı Odası B", Capacity = 10, IsDeleted = false },
            new Room { Name = "Konferans Salonu", Capacity = 20, IsDeleted = false }
        };

        await _context.Rooms.AddRangeAsync(rooms);
        await _context.SaveChangesAsync();
    }
}
