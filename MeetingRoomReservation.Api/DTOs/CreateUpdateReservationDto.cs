namespace MeetingRoomReservation.Api.DTOs
{
    public class CreateUpdateReservationDto
    {
        public int RoomId { get; set; }
        public int UserId { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public int ParticipantCount { get; set; }

        // Recurrence bilgisi
        public DayOfWeek? WeekDay { get; set; }
        public int? MonthDay { get; set; }
        public int? RecurrenceCount { get; set; }  // null ise tek rezervasyon
    }

}
