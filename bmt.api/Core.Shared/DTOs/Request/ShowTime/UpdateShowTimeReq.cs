namespace Core.Shared.DTOs.Request.ShowTime
{
    public class UpdateShowTimeReq
    {
        public int ShowtimeId { get; set; }
        public int RoomId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public double? Price { get; set; }
    }
}
