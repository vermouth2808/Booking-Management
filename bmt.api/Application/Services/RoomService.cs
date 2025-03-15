using Core.Application.Interfaces;
using Core.Shared.Common.Models;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Response.Room;
using Core.Shared.DTOs.Request.Room;

namespace Core.Application.Services
{
    public class RoomService<T> : IRoomService<T> where T : RoomRes, new()
    {
        private readonly IRoomRepository<T> _RoomRepository;

        public RoomService(IRoomRepository<T> RoomRepository)
        {
            _RoomRepository = RoomRepository;
        }
        /// <summary>
        /// read Room by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<T>> GetRoomById(int id)
        {
            var result = await _RoomRepository.GetRoomById(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return Result<T>.Failure("Room not found");
            }

            return Result<T>.Success(result.Data);
        }


     /*   /// <summary>
        /// seach Room
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<RoomSearchRes>> SearchRoom(SearchRoomReq req)
        {
            var result = await _RoomRepository.SearchRoom(req);
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<RoomSearchRes>.Failure("No Rooms found");
            }

            return Result<RoomSearchRes>.Success(result.Data);
        }*/
        /// <summary>
        /// create a Room
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<bool>> CreateRoom(CreateRoomReq req, int CreatedUserId)
        {
            var result = await _RoomRepository.CreateRoom(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Create a failed Room");
            }

            return Result<bool>.Success(true);
        }
        /// <summary>
        /// update a Room
        /// </summary>
        /// <param name="req"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> UpdateRoom(UpdateRoomReq req, int CreatedUserId)
        {
            var result = await _RoomRepository.UpdateRoom(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed Room");
            }
            return Result<bool>.Success(true);
        }
        /// <summary>
        /// remove a Room
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteRoom(int id, int CreatedUserId)
        {
            var result = await _RoomRepository.DeleteRoom(id, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed Room");
            }
            return Result<bool>.Success(true);
        }
    }
}
