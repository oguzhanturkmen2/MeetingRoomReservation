
    using MeetingRoomReservation.Api.Data;
    using MeetingRoomReservation.Api.DTOs;
    using MeetingRoomReservation.Api.Entities;
    using MeetingRoomReservation.Api.Services.Interfaces;
    using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Services
{   
    public class UserService : IUserService
    {
        private readonly AppDbContext _context;

        public UserService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<UserDto>> GetAllAsync()
        {
            return await _context.Users
                .Where(x => !x.IsDeleted)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email
                })
                .ToListAsync();
        }

        public async Task<UserDto?> GetByIdAsync(int id)
        {
            return await _context.Users
                .Where(x => x.Id == id && !x.IsDeleted)
                .Select(x => new UserDto
                {
                    Id = x.Id,
                    FullName = x.FullName,
                    Email = x.Email
                })
                .FirstOrDefaultAsync();
        }

        public async Task<int> CreateAsync(CreateUpdateUserDto dto)
        {
            var exists = await _context.Users
                .AnyAsync(x => x.Email == dto.Email && !x.IsDeleted);

            if (exists)
                throw new Exception("Email zaten var.");

            var user = new User
            {
                FullName = dto.FullName,
                Email = dto.Email
            };

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user.Id;
        }

        public async Task UpdateAsync(int id, CreateUpdateUserDto dto)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null || user.IsDeleted)
                throw new Exception("Kullanıcı bulunamadı.");

            user.FullName = dto.FullName;
            user.Email = dto.Email;

            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null) return;

            user.IsDeleted = true; // soft delete
            await _context.SaveChangesAsync();
        }
    }

}
