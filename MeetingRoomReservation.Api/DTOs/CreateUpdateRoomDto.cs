namespace MeetingRoomReservation.Api.DTOs
{
    public class CreateUpdateRoomDto
    {
        public string Name { get; set; } = null!;
        public int Capacity { get; set; }
        public List<int> EquipmentIds { get; set; } = new();
    }
}
