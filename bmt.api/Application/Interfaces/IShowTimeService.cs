using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.ShowTime;
using Core.Shared.DTOs.Response.ShowTime;

namespace Core.Application.Interfaces
{
    public interface IShowTimeService<T> where T : ShowTimeRes, new()
    {
        Task<Result<T>> GetShowTimeById(int id);
        Task<Result<ShowTimeSearchRes>> SearchShowTime(SearchShowTimeReq req);
        Task<Result<bool>> CreateShowTime(CreateShowTimeReq req, int CreatedUserId);
        Task<Result<bool>> UpdateShowTime(UpdateShowTimeReq req, int CreatedUserId);
        Task<Result<bool>> DeleteShowTime(int id, int CreatedUserId);
    }

}
