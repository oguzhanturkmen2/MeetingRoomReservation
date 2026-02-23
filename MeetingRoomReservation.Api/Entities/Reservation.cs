namespace MeetingRoomReservation.Api.Entities
{
    public class Reservation : BaseEntity
    {
        public int RoomId { get; set; }
        public Room Room { get; set; }

        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ParticipantCount { get; set; }

        public bool IsActive { get; set; } = true;

        public int? RecurrenceId { get; set; }
        public Recurrence Recurrence { get; set; }
    }
}
