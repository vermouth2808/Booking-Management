using Core.Shared.Common.Models;
using Core.Shared.DTOs.Response.Movie;

namespace Core.Application.Interfaces
{
    public interface IMovieService<T> where T : MovieRes, new()
    {
        Task<Result<MovieRes>> GetMovieById(int id);  
    }

}
