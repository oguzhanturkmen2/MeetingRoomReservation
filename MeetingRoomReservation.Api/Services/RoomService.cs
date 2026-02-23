using Microsoft.EntityFrameworkCore;
using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Entities;
using MeetingRoomReservation.Api.Services.Interfaces;

public class RoomService : IRoomService
{
    private readonly AppDbContext _context;

    public RoomService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<RoomDto>> GetAllAsync()
    {
        return await _context.Rooms
            .Where(x => x.IsActive)
            .Select(x => new RoomDto
            {
                Id = x.Id,
                Name = x.Name,
                Capacity = x.Capacity,
                Equipments = x.RoomEquipments
                    .Select(re => new EquipmentDto
                    {
                        Id = re.Equipment.Id,
                        Name = re.Equipment.Name,
                        Specification = re.Equipment.Specification
                    })
                    .ToList()
            })
            .ToListAsync();
    }


    public async Task<RoomDto?> GetByIdAsync(int id)
    {
        return await _context.Rooms
            .Where(x => x.Id == id && x.IsActive)
            .Select(x => new RoomDto
            {
                Id = x.Id,
                Name = x.Name,
                Capacity = x.Capacity,
                Equipments = x.RoomEquipments
                    .Select(re => new EquipmentDto
                    {
                        Id = re.Equipment.Id,
                        Name = re.Equipment.Name,
                        Specification = re.Equipment.Specification
                    })
                    .ToList()
            })
            .FirstOrDefaultAsync();
    }


    public async Task<int> CreateAsync(CreateUpdateRoomDto dto)
    {
        var room = new Room
        {
            Name = dto.Name,
            Capacity = dto.Capacity,
            RoomEquipments = new List<RoomEquipment>()
        };

        foreach (var equipmentId in dto.EquipmentIds)
        {
            room.RoomEquipments.Add(new RoomEquipment
            {
                EquipmentId = equipmentId
            });
        }

        _context.Rooms.Add(room);
        await _context.SaveChangesAsync();
        return room.Id;
    }


    public async Task UpdateAsync(int id, CreateUpdateRoomDto dto)
    {
        var room = await _context.Rooms
            .Include(r => r.RoomEquipments)
            .FirstOrDefaultAsync(r => r.Id == id);

        if (room == null)
            throw new Exception("Oda bulunamadı.");

        room.Name = dto.Name;
        room.Capacity = dto.Capacity;

        // Eski ekipmanları sil
        room.RoomEquipments.Clear();

        // Yenileri ekle
        foreach (var equipmentId in dto.EquipmentIds)
        {
            room.RoomEquipments.Add(new RoomEquipment
            {
                RoomId = room.Id,
                EquipmentId = equipmentId
            });
        }

        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var room = await _context.Rooms.FindAsync(id);

        if (room == null || !room.IsActive)
            throw new Exception("Oda bulunamadı.");

        var hasFutureReservations = await _context.Reservations
            .AnyAsync(r =>
                r.RoomId == id &&
                !r.IsDeleted &&
                r.StartDate > DateTime.Now);

        if (hasFutureReservations)
            throw new Exception("Bu odaya ait gelecekte rezervasyonlar var. Silinemez.");

        room.IsDeleted = true;

        await _context.SaveChangesAsync();
    }

}
