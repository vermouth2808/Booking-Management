namespace Core.Shared.DTOs.Response.Room
{
    public class RoomRes
    {
        public int RoomId { get; set; }
        public string RoomName { get; set; } = null!;
        public int TotalRows { get; set; }
        public int TotalCols { get; set; }
        public string? Layout { get; set; }
        public string? Description { get; set; }
    }
}
