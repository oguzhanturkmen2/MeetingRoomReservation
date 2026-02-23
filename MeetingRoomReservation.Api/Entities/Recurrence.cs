namespace MeetingRoomReservation.Api.Entities
{
    public class Recurrence : BaseEntity
    {
        public DayOfWeek? WeekDay { get; set; }   // Weekly pattern
        public int? MonthDay { get; set; }        // Monthly pattern

        public int Count { get; set; }            // Kaç tekrar

        public ICollection<Reservation> Reservations { get; set; }
    }
}
