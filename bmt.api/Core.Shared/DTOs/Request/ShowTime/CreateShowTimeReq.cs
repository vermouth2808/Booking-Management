namespace Core.Shared.DTOs.Request.ShowTime
{
    public class CreateShowTimeReq
    {

        public int RoomId { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public double? Price { get; set; }
    }
}
