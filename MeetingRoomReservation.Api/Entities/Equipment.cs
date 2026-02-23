namespace MeetingRoomReservation.Api.Entities
{
    public class Equipment : BaseEntity
    {
        public string Name { get; set; }
        public string? Specification { get; set; } = null;

    }
}
