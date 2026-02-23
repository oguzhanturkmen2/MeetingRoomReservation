namespace MeetingRoomReservation.Api.DTOs
{
    public class CreateRoomDto
    {
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
    }
}
