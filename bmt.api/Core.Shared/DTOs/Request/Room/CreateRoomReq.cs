namespace Core.Shared.DTOs.Request.Room
{
    public class CreateRoomReq
    {
        public string RoomName { get; set; } = null!;

        public string? Layout { get; set; }

        public string? Description { get; set; }
    }
}
