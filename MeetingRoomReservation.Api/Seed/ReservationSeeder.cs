using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.Entities;
using Microsoft.EntityFrameworkCore;
public class ReservationSeeder : IDataSeeder
{
    public int Order => 5;

    private readonly AppDbContext _context;

    public ReservationSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Reservations.AnyAsync())
            return;

        var reservations = new List<Reservation>
        {
            new Reservation
            {
                RoomId = 1,
                UserId = 1,
                StartDate = new DateTime(2026, 3, 1, 10, 0, 0),
                EndDate = new DateTime(2026, 3, 1, 11, 0, 0),
                ParticipantCount = 3,
                IsDeleted = false
            },
            new Reservation
            {
                RoomId = 2,
                UserId = 2,
                StartDate = new DateTime(2026, 3, 2, 14, 0, 0),
                EndDate = new DateTime(2026, 3, 2, 16, 0, 0),
                ParticipantCount = 6,
                RecurrenceId = null,
                IsDeleted = false
            },
            new Reservation
            {
                RoomId = 3,
                UserId = 3,
                StartDate = new DateTime(2026, 3, 3, 9, 0, 0),
                EndDate = new DateTime(2026, 3, 3, 12, 0, 0),
                ParticipantCount = 15,
                RecurrenceId = null,
                IsDeleted = false
            }
        };

        await _context.Reservations.AddRangeAsync(reservations);
        await _context.SaveChangesAsync();
    }
}
