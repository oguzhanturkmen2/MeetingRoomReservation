using Microsoft.EntityFrameworkCore;
using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Entities;
using MeetingRoomReservation.Api.Services.Interfaces;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;

    public ReservationService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .Where(x => x.IsActive)
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(x => x.Id == id && x.IsActive);
    }

    public async Task<int> CreateAsync(CreateReservationDto dto)
    {
        var conflict = await _context.Reservations
            .AnyAsync(r =>
                r.RoomId == dto.RoomId &&
                r.IsActive &&
                dto.StartDate < r.EndDate &&
                dto.EndDate > r.StartDate);

        if (conflict)
            throw new Exception("Toplantı odası bu saat için zaten rezerve edilmiş.");

        var reservation = new Reservation
        {
            RoomId = dto.RoomId,
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            IsActive = true
        };

        await _context.Reservations.AddAsync(reservation);
        await _context.SaveChangesAsync();

        return reservation.Id;
    }

    public async Task UpdateAsync(int id, CreateReservationDto dto)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null || !reservation.IsActive)
            throw new Exception("Reservasyon bulunamadı.");

        reservation.StartDate = dto.StartDate;
        reservation.EndDate = dto.EndDate;

        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var reservation = await _context.Reservations.FindAsync(id);
        if (reservation == null) return;

        reservation.IsActive = false;

        await _context.SaveChangesAsync();
    }
}

