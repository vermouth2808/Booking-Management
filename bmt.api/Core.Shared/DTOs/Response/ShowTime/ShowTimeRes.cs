namespace Core.Shared.DTOs.Response.ShowTime
{
    public class ShowTimeRes
    {
        public int ShowtimeId { get; set; }

        public int MovieId { get; set; }

        public string? Room { get; set; }

        public DateTime? StartTime { get; set; }

        public DateTime? EndTime { get; set; }

        public double? Price { get; set; }
    }
}
