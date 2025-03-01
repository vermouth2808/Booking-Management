using AutoMapper;
using Core.Domain.Entities;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Movie;
using Core.Shared.DTOs.Response.Movie;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Core.Infrastructure.Repositories
{
    public class MovieRepository<T> : IMovieRepository<T> where T : MovieRes, new()
    {
        private readonly BookMovieTicketContext _context;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public MovieRepository(BookMovieTicketContext context, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _context = context;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        public async Task<Result<bool>> CreateMovie(CreateMovieReq req, int CreatedUserId)
        {
            var movie = new Movie()
            {
                Title = req.Title,
                Director = req.Director,
                Performer = req.Performer,
                Language = req.Language,
                Genre = req.Genre,
                Duration = req.Duration,
                ReleaseDate = req.ReleaseDate,
                PosterUrl = req.PosterUrl,
                TrailerUrl = req.TrailerUrl,
                Description = req.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = CreatedUserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = CreatedUserId,
                IsDeleted = false,
            };
            _context.Movies.Add(movie);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveByPatternAsync("movies_");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteMovie(int id, int updatedUserId)
        {
            string cacheKey = $"movie_{id}";
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id && m.IsDeleted == false);
            if (movie == null)
            {
                return Result<bool>.Failure("Movie not found");
            }

            movie.IsDeleted = true;
            movie.UpdatedDate = DateTime.UtcNow;
            movie.UpdatedUserId = updatedUserId;

            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("movies_");
            await _redisCacheService.RemoveDataAsync(cacheKey);

            return Result<bool>.Success(true);
        }


        public async Task<Result<T>> GetMovieById(int id)
        {
            string cacheKey = $"movie_{id}";

            var cachedMovie = await _redisCacheService.GetDataAsync<Movie>(cacheKey);
            if (cachedMovie != null)
            {
                var mappedMovie = _mapper.Map<T>(cachedMovie);
                return Result<T>.Success(mappedMovie, "Successfully");
            }

            var efItem = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id && m.IsDeleted == false);
            if (efItem == null)
            {
                return Result<T>.Failure("Movie not found");
            }

            _redisCacheService.SetDataAsync(cacheKey, efItem, null);

            var mappedResult = _mapper.Map<T>(efItem);
            return Result<T>.Success(mappedResult, "Successfully");
        }

        public async Task<Result<MovieSearchRes>> SearchMovie(SearchMovieReq req)
        {
            string processedKeySearch = req.KeySearch?.Trim();
            processedKeySearch = Regex.Replace(processedKeySearch, @"\s+", " ");
            string cacheKey = $"movies_{req.KeySearch}_{req.PageIndex}_{req.PageSize}";

            var cachedMovies = await _redisCacheService.GetDataAsync<MovieSearchRes>(cacheKey);
            if (cachedMovies != null)
            {
                return Result<MovieSearchRes>.Success(cachedMovies, "Successfully retrieved from cache");
            }

            var query = _context.Movies.AsQueryable().Where(m => m.IsDeleted == false);

            if (!string.IsNullOrEmpty(req.KeySearch))
            {
                query = query.Where(m => m.Title.Contains(req.KeySearch));
            }

            int totalRecords = await query.CountAsync();

            var pageIndex = Math.Max(1, req.PageIndex);
            var movies = await query
                .Where(m => !m.IsDeleted)
                .OrderBy(m => m.CreatedDate)
                .Skip((pageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();



            if (!movies.Any())
            {
                return Result<MovieSearchRes>.Failure("No movies found");
            }

            // Mapping dữ liệu trước khi lưu cache & trả về
            var mappedMovies = movies.Select(_mapper.Map<T>);
            var searchResult = new MovieSearchRes
            {
                TotalRecords = totalRecords,
                Movies = mappedMovies
            };
            _redisCacheService.SetDataAsync(cacheKey, mappedMovies, null);

            return Result<MovieSearchRes>.Success(searchResult, "Successfully retrieved from database");
        }

        public async Task<Result<bool>> UpdateMovie(UpdateMovieReq req, int updatedUserId)
        {
            var movie = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == req.MovieId && m.IsDeleted == false);
            if (movie == null)
            {
                return Result<bool>.Failure("Movie not found");
            }
            string cacheKey = $"movie_{req.MovieId}";
            movie.Title = req.Title ?? movie.Title;
            movie.Director = req.Director ?? movie.Director;
            movie.Performer = req.Performer ?? movie.Performer;
            movie.Language = req.Language ?? movie.Language;
            movie.Genre = req.Genre ?? movie.Genre;
            movie.Duration = req.Duration ?? movie.Duration;
            movie.ReleaseDate = req.ReleaseDate ?? movie.ReleaseDate;
            movie.PosterUrl = req.PosterUrl ?? movie.PosterUrl;
            movie.TrailerUrl = req.TrailerUrl ?? movie.TrailerUrl;
            movie.Description = req.Description ?? movie.Description;
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("movies_");
            await _redisCacheService.RemoveDataAsync(cacheKey);
            return Result<bool>.Success(true);
        }
    }
}
