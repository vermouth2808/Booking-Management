using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Movie;
using Core.Shared.DTOs.Response.Movie;

namespace Core.Application.Interfaces
{
    public interface IMovieService<T> where T : MovieRes, new()
    {
        Task<Result<T>> GetMovieById(int id);
        Task<Result<MovieSearchRes>> SearchMovie(SearchMovieReq req);
        Task<Result<bool>> CreateMovie(CreateMovieReq req, int CreatedUserId);
        Task<Result<bool>> UpdateMovie(UpdateMovieReq req, int CreatedUserId);
        Task<Result<bool>> DeleteMovie(int id, int CreatedUserId);
    }

}
