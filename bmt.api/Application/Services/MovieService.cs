using Core.Application.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Movie;
using Core.Domain.Entities;
using Core.Infrastructure.Repositories;
using Core.Infrastructure.Mappings;

namespace Core.Application.Services
{
    public class MovieService<T> : IMovieService<T> where T : MovieRes, new()
    {
        private readonly IMovieRepository<T> _movieRepository;
        private readonly IMovieMapper<T> _movieMapper;

        public MovieService(IMovieRepository<T> movieRepository, IMovieMapper<T> movieMapper)
        {
            _movieRepository = movieRepository;
            _movieMapper = movieMapper;
        }

        public async Task<Result<MovieRes>> GetMovieById(int id)
        {
            // Lấy phim từ repository
            var result = await _movieRepository.GetMovieById(id);

            if (!result.IsSuccess)
            {
                return Result<MovieRes>.Failure("Movie not found"); // Trả về lỗi nếu không tìm thấy phim
            }

            // Chuyển đổi dữ liệu từ model Movie (Entity) sang kiểu T (MovieRes hoặc kiểu khác)
            var movieRes = _movieMapper.ToModel(result.Data);

            return Result<MovieRes>.Success(movieRes); // Trả về kết quả thành công
        }
    }
}
