using Core.Application.Interfaces;
using Core.Shared.Common.Models;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Response.Movie;
using Core.Shared.DTOs.Request.Movie;

namespace Core.Application.Services
{
    public class MovieService<T> : IMovieService<T> where T : MovieRes, new()
    {
        private readonly IMovieRepository<T> _movieRepository;

        public MovieService(IMovieRepository<T> movieRepository)
        {
            _movieRepository = movieRepository;
        }
        /// <summary>
        /// read movie by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<T>> GetMovieById(int id)
        {
            var result = await _movieRepository.GetMovieById(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return Result<T>.Failure("Movie not found");
            }

            return Result<T>.Success(result.Data);
        }


        /// <summary>
        /// seach movie
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<MovieSearchRes>> SearchMovie(SearchMovieReq req)
        {
            var result = await _movieRepository.SearchMovie(req);
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<MovieSearchRes>.Failure("No movies found");
            }

            return Result<MovieSearchRes>.Success(result.Data);
        }
        /// <summary>
        /// create a movie
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<bool>> CreateMovie(CreateMovieReq req, int CreatedUserId)
        {
            var result = await _movieRepository.CreateMovie(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Create a failed movie");
            }

            return Result<bool>.Success(true);
        }
        /// <summary>
        /// update a movie
        /// </summary>
        /// <param name="req"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> UpdateMovie(UpdateMovieReq req, int CreatedUserId)
        {
            var result = await _movieRepository.UpdateMovie(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed movie");
            }
            return Result<bool>.Success(true);
        }
        /// <summary>
        /// remove a movie
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteMovie(int id, int CreatedUserId)
        {
            var result = await _movieRepository.DeleteMovie(id, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed movie");
            }
            return Result<bool>.Success(true);
        }
    }
}
