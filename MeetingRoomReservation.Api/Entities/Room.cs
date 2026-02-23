namespace MeetingRoomReservation.Api.Entities
{
    public class Room : BaseEntity
    {
        public string Name { get; set; }
        public int Capacity { get; set; }
        public int Floor { get; set; }
        public bool IsActive { get; set; } = true;
        public ICollection<RoomEquipment> RoomEquipments { get; set; }
        public ICollection<Reservation> Reservations { get; set; }
    }
}
