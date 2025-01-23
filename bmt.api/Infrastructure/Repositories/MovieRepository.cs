using Core.Domain.Entities;
using Core.Infrastructure.Mappings;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Movie;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories
{
    public interface IMovieRepository<T> where T : MovieRes, new()
    {
        Task<Result<T>> GetMovieById(int id);
    }

    public class MovieRepository<T> : IMovieRepository<T> where T : MovieRes, new() 
    {
        private readonly BookMovieTicketContext _context;
        private readonly IMovieMapper<T> _mapper;

        public MovieRepository(BookMovieTicketContext context, IMovieMapper<T> mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result<T>> GetMovieById(int id)
        {
            var efItem = await _context.Movies.FirstOrDefaultAsync(m => m.MovieId == id);

            if (efItem == null)
            {
                return await Task.FromResult(Result<T>.Failure("Movie not found"));
            }

            var mappedMovie = _mapper.ToModel(efItem); 

            return await Task.FromResult(Result<T>.Success(mappedMovie, "Successfully"));
        }
    }
}
