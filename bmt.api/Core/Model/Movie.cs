namespace Core.Repositories.Model
{
    public class Movie {
        public int MovieId { get; set; }

        public string Title { get; set; } = null!;
        public string? Genre { get; set; }
        public int? Duration { get; set; }
        public DateOnly? ReleaseDate { get; set; }

        public string? PosterUrl { get; set; }

        public string? TrailerUrl { get; set; }

        public string? Description { get; set; }

      /*  public DateTime CreatedDate { get; set; }

        public int CreatedUserId { get; set; }

        public DateTime? UpdatedDate { get; set; }

        public int? UpdatedUserId { get; set; }

        public bool IsDeleted { get; set; }*/
    }
}
