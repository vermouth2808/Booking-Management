export interface MovieModelRes {
  totalRecords: number;
  movies: Movie[];
}

export interface Movie {
    movieId: number;
    title: string;
    director?: string;
    performer?: string;
    language?: string;
    /** Thể loại */
    genre?: string;
    /** Thời lượng phim */
    duration?: number;
    /** Ngày phát hành phim */
    releaseDate?: string; // DateOnly không có trong TS, có thể dùng string hoặc Date
    posterUrl?: string;
    trailerUrl?: string;
    ageRating?: string;
    description?: string;
  }