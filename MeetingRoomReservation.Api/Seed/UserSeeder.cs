using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.Entities;
using Microsoft.EntityFrameworkCore;

public class UserSeeder : IDataSeeder
{
    private readonly AppDbContext _context;
    public int Order => 1;

    public UserSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Users.AnyAsync())
            return;

        var users = new List<User>
        {
            new User { FullName = "Ali",Email="ali@hitsoft.com", IsDeleted = false },
            new User { FullName = "Ayşe",Email="ayse@hitsoft.com", IsDeleted = false },
            new User { FullName = "Mehmet",Email="mehmet@hitsoft.com", IsDeleted = false }
        };

        await _context.Users.AddRangeAsync(users);
        await _context.SaveChangesAsync();
    }
}
