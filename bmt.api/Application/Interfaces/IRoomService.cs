using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Room;
using Core.Shared.DTOs.Response.Room;

namespace Core.Application.Interfaces
{
    public interface IRoomService<T> where T : RoomRes, new()
    {
        Task<Result<T>> GetRoomById(int id);
        // Task<Result<RoomSearchRes>> SearchRoom(SearchRoomReq req);
        Task<Result<bool>> CreateRoom(CreateRoomReq req, int CreatedUserId);
        Task<Result<bool>> UpdateRoom(UpdateRoomReq req, int CreatedUserId);
        Task<Result<bool>> DeleteRoom(int id, int CreatedUserId);
    }

}
