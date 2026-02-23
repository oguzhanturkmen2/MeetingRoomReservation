using MeetingRoomReservation.Api.Data;
using MeetingRoomReservation.Api.Entities;
using MeetingRoomReservation.Api.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Services
{
    public class PublicHolidayService : IPublicHolidayService
    {
        private readonly AppDbContext _context;

        public PublicHolidayService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<DateTime>> GetAllAsync()
        {
            return await _context.PublicHolidays
                .Select(x => x.HolidayDate)
                .ToListAsync();
        }

        public async Task<DateTime?> GetByIdAsync(int id)
        {
            var entity = await _context.PublicHolidays.FindAsync(id);

            return entity?.HolidayDate;
        }

        public async Task CreateAsync(DateTime date)
        {
            var exists = await _context.PublicHolidays
                .AnyAsync(x => x.HolidayDate.Date == date.Date);

            if (exists)
                throw new Exception("Bu tarihte zaten resmi tatil tanımlı.");

            var entity = new PublicHoliday
            {
                HolidayDate = date.Date
            };

            _context.PublicHolidays.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, DateTime date)
        {
            var entity = await _context.PublicHolidays.FindAsync(id);

            if (entity == null)
                throw new Exception("Resmi tatil bulunamadı.");

            entity.HolidayDate = date.Date;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.PublicHolidays.FindAsync(id);

            if (entity == null)
                throw new Exception("Resmi tatil bulunamadı.");

            _context.PublicHolidays.Remove(entity);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> IsPublicHolidayAsync(DateTime date)
        {
            return await _context.PublicHolidays
                .AnyAsync(x => x.HolidayDate.Date == date.Date);
        }
    }
}