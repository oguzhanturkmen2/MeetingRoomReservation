using MeetingRoomReservation.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace MeetingRoomReservation.Api.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<Room> Rooms { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Reservation> Reservations { get; set; }
        public DbSet<Equipment> Equipments { get; set; }
        public DbSet<RoomEquipment> RoomEquipments { get; set; }
        public DbSet<Recurrence> Recurrences { get; set; }
        public DbSet<PublicHoliday> PublicHolidays { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<RoomEquipment>()
                .HasKey(re => new { re.RoomId, re.EquipmentId });

            modelBuilder.Entity<Room>()
                .HasIndex(r => r.Name)
                .IsUnique();

            modelBuilder.Entity<Reservation>()
                .HasIndex(r => new { r.RoomId, r.StartDate, r.EndDate });

            // Soft delete global filter
            modelBuilder.Entity<Room>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<User>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Reservation>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Equipment>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<Recurrence>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<PublicHoliday>().HasQueryFilter(x => !x.IsDeleted);
            modelBuilder.Entity<RoomEquipment>()
                .HasQueryFilter(re => !re.Room.IsDeleted && !re.Equipment.IsDeleted);

        }
    }
}
