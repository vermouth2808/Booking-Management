﻿namespace Core.Shared.DTOs.Request.Movie
{
    public class CreateMovieReq
    {

        public string Title { get; set; } = null!;
        public string? Director { get; set; }
        public string? Performer { get; set; }
        public string? Language { get; set; }
        /// <summary>
        /// thể loại
        /// </summary>
        public string? Genre { get; set; }

        /// <summary>
        /// thời lượng phim
        /// </summary>
        public int? Duration { get; set; }

        /// <summary>
        /// ngày phát hành phim
        /// </summary>
        public DateOnly? ReleaseDate { get; set; }

        public string? PosterUrl { get; set; }

        public string? TrailerUrl { get; set; }

        public string? Description { get; set; }
    }
}
