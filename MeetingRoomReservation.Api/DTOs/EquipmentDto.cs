namespace MeetingRoomReservation.Api.DTOs
{
    public class EquipmentDto
    {
        public int Id { get; set; }
        public string Name { get; set; } = null!;
        public string? Specification { get; set; }
    }
}
