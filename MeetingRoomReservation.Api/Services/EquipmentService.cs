namespace MeetingRoomReservation.Api.Services
{
    using MeetingRoomReservation.Api.Data;
    using MeetingRoomReservation.Api.Entities;
    using MeetingRoomReservation.Api.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

    public class EquipmentService : IEquipmentService
    {
        private readonly AppDbContext _context;

        public EquipmentService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Equipment>> GetAllAsync()
        {
            return await _context.Equipments
                .OrderBy(x => x.Name)
                .ToListAsync();
        }

        public async Task<Equipment?> GetByIdAsync(int id)
        {
            return await _context.Equipments.FindAsync(id);
        }

        public async Task CreateAsync(string name, string? specification)
        {
            var exists = await _context.Equipments
                .AnyAsync(x => x.Name.ToLower() == name.ToLower());

            if (exists)
                throw new Exception("Bu isimde ekipman zaten mevcut.");

            var entity = new Equipment
            {
                Name = name,
                Specification = specification
            };

            _context.Equipments.Add(entity);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(int id, string name, string? specification)
        {
            var entity = await _context.Equipments.FindAsync(id);

            if (entity == null)
                throw new Exception("Ekipman bulunamadı.");

            entity.Name = name;
            entity.Specification = specification;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = await _context.Equipments.FindAsync(id);

            if (entity == null)
                throw new Exception("Ekipman bulunamadı.");

            _context.Equipments.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }

}
