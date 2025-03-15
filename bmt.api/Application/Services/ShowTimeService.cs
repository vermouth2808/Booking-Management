using Core.Application.Interfaces;
using Core.Shared.Common.Models;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Response.ShowTime;
using Core.Shared.DTOs.Request.ShowTime;

namespace Core.Application.Services
{
    public class ShowTimeService<T> : IShowTimeService<T> where T : ShowTimeRes, new()
    {
        private readonly IShowTimeRepository<T> _ShowTimeRepository;

        public ShowTimeService(IShowTimeRepository<T> ShowTimeRepository)
        {
            _ShowTimeRepository = ShowTimeRepository;
        }
        /// <summary>
        /// read ShowTime by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<T>> GetShowTimeById(int id)
        {
            var result = await _ShowTimeRepository.GetShowTimeById(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return Result<T>.Failure("ShowTime not found");
            }

            return Result<T>.Success(result.Data);
        }


        /// <summary>
        /// seach ShowTime
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<ShowTimeSearchRes>> SearchShowTime(SearchShowTimeReq req)
        {
            var result = await _ShowTimeRepository.SearchShowTime(req);
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<ShowTimeSearchRes>.Failure("No ShowTimes found");
            }

            return Result<ShowTimeSearchRes>.Success(result.Data);
        }
        /// <summary>
        /// create a ShowTime
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<bool>> CreateShowTime(CreateShowTimeReq req, int CreatedUserId)
        {
            var result = await _ShowTimeRepository.CreateShowTime(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Create a failed ShowTime");
            }

            return Result<bool>.Success(true);
        }
        /// <summary>
        /// update a ShowTime
        /// </summary>
        /// <param name="req"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> UpdateShowTime(UpdateShowTimeReq req, int CreatedUserId)
        {
            var result = await _ShowTimeRepository.UpdateShowTime(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed ShowTime");
            }
            return Result<bool>.Success(true);
        }
        /// <summary>
        /// remove a ShowTime
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteShowTime(int id, int CreatedUserId)
        {
            var result = await _ShowTimeRepository.DeleteShowTime(id, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed ShowTime");
            }
            return Result<bool>.Success(true);
        }
    }
}
