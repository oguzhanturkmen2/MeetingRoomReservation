using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.DTOs;
using MeetingRoomReservation.Api.Entities;
using MeetingRoomReservation.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;

public class ReservationService : IReservationService
{
    private readonly AppDbContext _context;
    private readonly IPublicHolidayService _holidayService;


    public ReservationService(AppDbContext context, IPublicHolidayService holidayService)
    {
        _context = context;
        _holidayService = holidayService;
    }

    public async Task<List<Reservation>> GetAllAsync()
    {
        return await _context.Reservations
            .ToListAsync();
    }

    public async Task<Reservation?> GetByIdAsync(int id)
    {
        return await _context.Reservations
            .FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task<int> CreateAsync(CreateUpdateReservationDto dto)
    {
        ValidateBusinessRules(dto);

        using var transaction = await _context.Database.BeginTransactionAsync();

        Recurrence recurrence = null;

        if (dto.RecurrenceCount.HasValue)
        {
            recurrence = new Recurrence
            {
                WeekDay = dto.WeekDay,
                MonthDay = dto.MonthDay,
                Count = dto.RecurrenceCount.Value
            };

            await _context.Recurrences.AddAsync(recurrence);
            await _context.SaveChangesAsync();
        }

        var reservations = await GenerateReservations(dto, recurrence);

        if (!reservations.Any())
            throw new Exception("Oluşturulabilir uygun rezervasyon bulunamadı.");

        await _context.Reservations.AddRangeAsync(reservations);
        await _context.SaveChangesAsync();

        await transaction.CommitAsync();
        return reservations.First().Id;
    }

    private void ValidateBusinessRules(CreateUpdateReservationDto dto)
    {
        if (dto.StartDate >= dto.EndDate)
            throw new Exception("Başlangıç tarihi bitişten büyük olamaz.");

        if (dto.StartDate < DateTime.Now)
            throw new Exception("Geçmişe rezervasyon yapılamaz.");

        if (dto.StartDate > DateTime.Now.AddMonths(3))
            throw new Exception("En fazla 3 ay ileriye rezervasyon yapılabilir.");

        var duration = dto.EndDate - dto.StartDate;

        if (duration.TotalMinutes < 30)
            throw new Exception("Minimum rezervasyon süresi 30 dakikadır.");

        if (duration.TotalHours > 4)
            throw new Exception("Maksimum rezervasyon süresi 4 saattir.");
    }

    private async Task<List<Reservation>> GenerateReservations(
        CreateUpdateReservationDto dto,
        Recurrence recurrence)
    {
        var list = new List<Reservation>();

        int repeat = recurrence?.Count ?? 1;

        DateTime currentStart = dto.StartDate;
        DateTime currentEnd = dto.EndDate;

        for (int i = 0; i < repeat; i++)
        {
            // 3 ay sınırı (recurrence için)
            if (currentStart > DateTime.Now.AddMonths(3))
                break;

            var IsPublicHoliday = await _holidayService.IsPublicHolidayAsync(currentStart);
            if (!IsWeekend(currentStart) && !IsPublicHoliday)
            {

                await ValidateRoomCapacity(dto.RoomId, dto.ParticipantCount);
                await ValidateUserAvailability(dto.UserId, dto.StartDate, dto.EndDate);
                await ValidateRoomAvailability(dto.RoomId, dto.StartDate, dto.EndDate);

                list.Add(new Reservation
                {
                    RoomId = dto.RoomId,
                    UserId = dto.UserId,
                    StartDate = currentStart,
                    EndDate = currentEnd,
                    ParticipantCount = dto.ParticipantCount,
                    RecurrenceId = recurrence?.Id
                });
            }

            // Weekly
            if (recurrence?.WeekDay != null)
            {
                currentStart = currentStart.AddDays(7);
                currentEnd = currentEnd.AddDays(7);
            }
            // Monthly
            else if (recurrence?.MonthDay != null)
            {
                currentStart = currentStart.AddMonths(1);
                currentEnd = currentEnd.AddMonths(1);
            }
        }

        return list;
    }

    private bool IsWeekend(DateTime date)
    {
        return date.DayOfWeek == DayOfWeek.Saturday ||
               date.DayOfWeek == DayOfWeek.Sunday;
    }



    public async Task UpdateAsync(int id, CreateUpdateReservationDto dto)
    {
        ValidateBusinessRules(dto);

        var reservation = await _context.Reservations
            .Include(r => r.Recurrence)
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (reservation == null)
            throw new Exception("Rezervasyon bulunamadı.");

        using var transaction = await _context.Database.BeginTransactionAsync();

        // Eğer recurrence yoksa zaten single update
        if (reservation.RecurrenceId == null)
        {
            await UpdateSingleReservation(reservation, dto);
        }
        else
        {
            await UpdateAllSeries(reservation, dto);
        }

        await transaction.CommitAsync();
    }

    private async Task UpdateSingleReservation(Reservation reservation, CreateUpdateReservationDto dto)
    {
        bool overlap = await _context.Reservations.AnyAsync(r =>
            r.Id != reservation.Id &&
            r.RoomId == dto.RoomId &&
            r.StartDate < dto.EndDate &&
            r.EndDate > dto.StartDate);

        if (overlap)
            throw new Exception("Rezervasyon çakışması var.");

        reservation.StartDate = dto.StartDate;
        reservation.EndDate = dto.EndDate;
        reservation.ParticipantCount = dto.ParticipantCount;

        await _context.SaveChangesAsync();
    }

    private async Task UpdateAllSeries(Reservation reservation, CreateUpdateReservationDto dto)
    {
        var series = await _context.Reservations
            .Where(r => r.RecurrenceId == reservation.RecurrenceId && !r.IsDeleted)
            .ToListAsync();

        foreach (var item in series)
        {
            item.StartDate = item.StartDate.Date
                .Add(dto.StartDate.TimeOfDay);

            item.EndDate = item.StartDate
                .Add(dto.EndDate - dto.StartDate);

            item.ParticipantCount = dto.ParticipantCount;
        }

        await _context.SaveChangesAsync();
    }


    public async Task DeleteAsync(int id)
    {
        var reservation = await _context.Reservations
            .FirstOrDefaultAsync(r => r.Id == id && !r.IsDeleted);

        if (reservation == null)
            return;

        ValidateCancellation(reservation);

        using var transaction = await _context.Database.BeginTransactionAsync();

        if (reservation.RecurrenceId == null)
        {
            reservation.IsDeleted = true;
        }
        else
        {
            var series = await _context.Reservations
                .Where(r => r.RecurrenceId == reservation.RecurrenceId &&
                            r.StartDate >= reservation.StartDate &&
                            !r.IsDeleted)
                .ToListAsync();

            foreach (var item in series)
            {
                ValidateCancellation(item);
                item.IsDeleted = true;
            }
        }

        await _context.SaveChangesAsync();
        await transaction.CommitAsync();
    }


    public async Task<bool> HasConflictAsync(int roomId, DateTime start, DateTime end)
    {
        return await _context.Reservations
            .AnyAsync(r =>
                r.RoomId == roomId &&
                !r.IsDeleted &&
                start < r.EndDate &&
                end > r.StartDate);
    }

    private async Task ValidateRoomCapacity(int roomId, int participantCount)
    {
        var room = await _context.Rooms
            .FirstOrDefaultAsync(r => r.Id == roomId);

        if (room == null)
            throw new Exception("Oda bulunamadı.");

        if (participantCount > room.Capacity)
            throw new Exception("Katılımcı sayısı oda kapasitesini aşamaz.");
    }

    private async Task ValidateUserAvailability(
    int userId,
    DateTime start,
    DateTime end,
    int? ignoreReservationId = null)
    {
        var conflict = await _context.Reservations
            .AnyAsync(r =>
                r.UserId == userId &&
                !r.IsDeleted &&
                (ignoreReservationId == null || r.Id != ignoreReservationId) &&
                r.StartDate < end &&
                r.EndDate > start);

        if (conflict)
            throw new Exception("Kullanıcı aynı anda birden fazla rezervasyon yapamaz.");
    }

    private async Task ValidateRoomAvailability(
    int roomId,
    DateTime start,
    DateTime end,
    int? ignoreReservationId = null)
    {
        var conflict = await _context.Reservations
            .AnyAsync(r =>
                r.RoomId == roomId &&
                !r.IsDeleted &&
                (ignoreReservationId == null || r.Id != ignoreReservationId) &&
                r.StartDate < end &&
                r.EndDate > start);

        if (conflict)
            throw new Exception("Oda belirtilen saat aralığında dolu.");
    }

    private void ValidateCancellation(Reservation reservation)
    {
        var now = DateTime.Now;

        if (reservation.StartDate <= now)
            throw new Exception("Başlamış veya geçmiş toplantı iptal edilemez.");

        if (reservation.StartDate <= now.AddMinutes(15))
            throw new Exception("Rezervasyon en geç 15 dakika öncesine kadar iptal edilebilir.");
    }

}

