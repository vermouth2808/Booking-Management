using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.FoodOrder;
using Core.Shared.DTOs.Response.FoodOrder;

namespace Core.Infrastructure.Repositories.Interfaces
{
    public interface IFoodOrderRepository<T> where T : FoodOrderRes, new()
    {
        Task<Result<T>> GetFoodOrderById(int id);
        Task<Result<FoodOrderSearchRes>> SearchFoodOrder(SearchFoodOrderReq req);
        Task<Result<bool>> CreateFoodOrder(CreateFoodOrderReq req, int CreatedUserId);
        Task<Result<bool>> UpdateFoodOrder(UpdateFoodOrderReq req, int CreatedUserId);
        Task<Result<bool>> DeleteFoodOrder(int id, int CreatedUserId);

    }

}
