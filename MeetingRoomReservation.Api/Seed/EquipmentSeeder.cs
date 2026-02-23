using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.Entities;
using Microsoft.EntityFrameworkCore;

public class EquipmentSeeder : IDataSeeder
{
    private readonly AppDbContext _context;

    public EquipmentSeeder(AppDbContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        if (await _context.Equipments.AnyAsync())
            return;

        var equipments = new List<Equipment>
        {
            new Equipment { Name = "Projeksiyon", Specification = "Full HD", IsDeleted = false },
            new Equipment { Name = "TV", Specification = "65 inch", IsDeleted = false },
            new Equipment { Name = "Beyaz Tahta", Specification = null, IsDeleted = false }
        };

        await _context.Equipments.AddRangeAsync(equipments);
        await _context.SaveChangesAsync();
    }
}
