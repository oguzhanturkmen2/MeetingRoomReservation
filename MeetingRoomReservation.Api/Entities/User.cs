namespace MeetingRoomReservation.Api.Entities
{
    public class User : BaseEntity
    {
        public string FullName { get; set; }
        public string Email { get; set; }

        public ICollection<Reservation> Reservations { get; set; }
    }
}
