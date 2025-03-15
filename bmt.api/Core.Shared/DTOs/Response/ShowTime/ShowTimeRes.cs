namespace Core.Shared.DTOs.Response.ShowTime
{
    public class ShowTimeRes
    {
        public int ShowtimeId { get; set; }
        public int MovieId { get; set; }
        public int RoomId { get; set; }
        public DateTime? StartTime { get; set; }
        public DateTime? EndTime { get; set; }
        public double? Price { get; set; }
        public string Title { get; set; } = null!;
        public string Director { get; set; } = null!;
        public string Performer { get; set; } = null!;
        public string Language { get; set; } = null!;
        public string Genre { get; set; } = null!;
        public int Duration { get; set; }
        public DateTime ReleaseDate { get; set; }
        public string PosterUrl { get; set; } = null!;
        public string TrailerUrl { get; set; } = null!;
        public string AgeRating { get; set; } = null!;
        public string RoomName { get; set; } = null!;
    }
}
