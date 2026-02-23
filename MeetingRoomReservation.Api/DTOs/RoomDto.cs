namespace MeetingRoomReservation.Api.DTOs
{
    public class RoomDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
    }
}
