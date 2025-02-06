using Core.Shared.Common.Models;
using Core.Shared.DTOs.Movie;

namespace Core.Infrastructure.Repositories.Interfaces
{
    public interface IMovieRepository<T> where T : MovieRes, new()
    {
        Task<Result<T>> GetMovieById(int id);
    }

}
