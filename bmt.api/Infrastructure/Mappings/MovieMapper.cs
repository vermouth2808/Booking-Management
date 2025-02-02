using Core.Domain.Entities;
using Core.Shared.DTOs.Movie;

namespace Core.Infrastructure.Mappings
{
    public interface IMovieMapper<T> where T : MovieRes, new()
    {
        T ToModel(Movie dataModel);
        Movie ToDataModel(T model);
        MovieRes ToModel<T>(T data) where T : MovieRes, new(); 
    }

    public class MovieMapper<T> : IMovieMapper<T> where T : MovieRes, new()
    {
        public MovieMapper() { }

        // Chuyển từ MovieRes (DTO) sang Movie (Entity)
        public Movie ToDataModel(T model)
        {
            var result = new Movie
            {
                MovieId = model.MovieId,
                Title = model.Title,
                Genre = model.Genre,
                Duration = model.Duration,
                ReleaseDate = model.ReleaseDate,
                PosterUrl = model.PosterUrl,
                TrailerUrl = model.TrailerUrl,
                Description = model.Description,
                CreatedDate = DateTime.Now,
                CreatedUserId = 1,
                IsDeleted = false,
            };

            return result;
        }

        // Chuyển từ Movie (Entity) sang MovieRes (DTO)
        public T ToModel(Movie dataModel)
        {
            var result = new T
            {
                MovieId = dataModel.MovieId,
                Title = dataModel.Title,
                Genre = dataModel.Genre,
                Duration = dataModel.Duration,
                ReleaseDate = dataModel.ReleaseDate,
                PosterUrl = dataModel.PosterUrl,
                TrailerUrl = dataModel.TrailerUrl,
                Description = dataModel.Description,
            };

            return result;
        }

        public MovieRes ToModel<T>(T data) where T : MovieRes, new()
        {
            return new MovieRes
            {
                MovieId = data.MovieId,
                Title = data.Title,
                Genre = data.Genre,
                Duration = data.Duration,
                ReleaseDate = data.ReleaseDate,
                PosterUrl = data.PosterUrl,
                TrailerUrl = data.TrailerUrl,
                Description = data.Description
            };
        }
    }
}
