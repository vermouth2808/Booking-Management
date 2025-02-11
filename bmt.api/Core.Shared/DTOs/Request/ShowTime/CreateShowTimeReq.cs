namespace Core.Shared.DTOs.Request.ShowTime
{
    public class CreateShowTimeReq
    {

        public string? Room { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public double? Price { get; set; }
    }
}
