namespace MeetingRoomReservation.Api.DTOs
{
    public class CreateUpdateReservationDto
    {
        public int RoomId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
