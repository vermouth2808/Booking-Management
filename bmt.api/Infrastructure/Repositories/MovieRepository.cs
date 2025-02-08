using Core.Domain.Entities;
using Core.Infrastructure.Mappings;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Response.Movie;
using Microsoft.EntityFrameworkCore;

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

        public async Task<Result<T>> GetMovieById(int id)
        {
            var efItem = _redisCacheService.GetData<Movie>("movie");
            if (efItem is null)
            {
                efItem = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);
                _redisCacheService.SetData("movie", efItem);
            }

            if (efItem == null)
            {
                return await Task.FromResult(Result<T>.Failure("Movie not found"));
            }

            var mappedMovie = _mapper.ToModel(efItem);

            return await Task.FromResult(Result<T>.Success(mappedMovie, "Successfully"));
        }
    }
}
