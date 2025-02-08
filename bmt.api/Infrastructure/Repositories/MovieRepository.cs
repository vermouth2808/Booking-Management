using Core.Domain.Entities;
using Core.Infrastructure.Mappings;
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
        private readonly IMovieMapper<T> _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public MovieRepository(BookMovieTicketContext context, IMovieMapper<T> mapper, IRedisCacheService redisCacheService)
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

        public async Task<Result<T>> GetMovieById(int id)
        {
            string cacheKey = $"movie_{id}";

            var cachedMovie =await _redisCacheService.GetDataAsync<Movie>(cacheKey);
            if (cachedMovie != null)
            {
                var mappedMovie = _mapper.ToModel(cachedMovie);
                return Result<T>.Success(mappedMovie, "Successfully");
            }

            var efItem = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
            if (efItem == null)
            {
                return Result<T>.Failure("Movie not found");
            }

            _redisCacheService.SetDataAsync(cacheKey, efItem, null);

            var mappedResult = _mapper.ToModel(efItem);
            return Result<T>.Success(mappedResult, "Successfully");
        }

        public async Task<Result<MovieSearchRes>> SearchMovie(SearchMovieReq req)
        {
            string processedKeySearch = req.KeySearch?.Trim();
            processedKeySearch = Regex.Replace(processedKeySearch, @"\s+", " ");
            string cacheKey = $"movies_{req.KeySearch}_{req.PageIndex}_{req.PageSize}";

            var cachedMovies =await _redisCacheService.GetDataAsync<MovieSearchRes>(cacheKey);
            if (cachedMovies != null)
            {
                return Result<MovieSearchRes>.Success(cachedMovies, "Successfully retrieved from cache");
            }

            var query = _context.Movies.AsQueryable();

            if (!string.IsNullOrEmpty(req.KeySearch))
            {
                query = query.Where(m => m.Title.Contains(req.KeySearch));
            }

            // Lấy tổng số lượng kết quả để hỗ trợ phân trang (nếu cần)
            int totalRecords = await query.CountAsync();

            var movies = await query
                .OrderBy(m => m.CreatedDate)
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();

            if (!movies.Any())
            {
                return Result<MovieSearchRes>.Failure("No movies found");
            }

            // Mapping dữ liệu trước khi lưu cache & trả về
            var mappedMovies = movies.Select(_mapper.ToModel);
            var searchResult = new MovieSearchRes
            {
                TotalRecords = totalRecords,
                Movies = mappedMovies
            };
            _redisCacheService.SetDataAsync(cacheKey, mappedMovies, null);

            return Result<MovieSearchRes>.Success(searchResult, "Successfully retrieved from database");
        }

    }
}
