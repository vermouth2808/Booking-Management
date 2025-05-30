﻿using System;
using System.Collections.Generic;

namespace Core.Domain.Entities;

public partial class Movie
{
    public int MovieId { get; set; }

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

    public string? AgeRating { get; set; }

    public string? Description { get; set; }

    public DateTime CreatedDate { get; set; }

    public int CreatedUserId { get; set; }

    public DateTime? UpdatedDate { get; set; }

    public int? UpdatedUserId { get; set; }

    public bool IsDeleted { get; set; }
    public ICollection<Showtime> Showtimes { get; set; }
}
