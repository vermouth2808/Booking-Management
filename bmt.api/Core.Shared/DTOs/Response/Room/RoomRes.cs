namespace Core.Shared.DTOs.Response.Room
{
    public class RoomRes
    {
        public int RoomId { get; set; }

        public string RoomName { get; set; } = null!;

        public string? Description { get; set; }
    }
}
