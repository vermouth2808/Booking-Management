using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.FoodCombo;
using Core.Shared.DTOs.Response.FoodCombo;

namespace Core.Infrastructure.Repositories.Interfaces
{
    public interface IFoodComboRepository<T> where T : FoodComboRes, new()
    {
        Task<Result<T>> GetFoodComboById(int id);
        Task<Result<IEnumerable<T>>> ReadAllFoodCombo();
        Task<Result<bool>> CreateFoodCombo(CreateFoodComboReq req, int CreatedUserId);
        Task<Result<bool>> UpdateFoodCombo(UpdateFoodComboReq req, int CreatedUserId);
        Task<Result<bool>> DeleteFoodCombo(int id, int CreatedUserId);

    }

}
