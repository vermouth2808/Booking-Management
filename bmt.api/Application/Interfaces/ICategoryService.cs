using Core.Application.Services;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Category;
using Core.Shared.DTOs.Response.Category;

namespace Core.Application.Interfaces
{
    public interface ICategoryService<T> where T : CategoryRes, new()
    {
        Task<Result<T>> GetCategoryById(int id);
        Task<Result<IEnumerable<T>>> ReadAllCategory();
        Task<Result<bool>> CreateCategory(CreateCategoryReq req, int CreatedUserId);
        Task<Result<bool>> UpdateCategory(UpdateCategoryReq req, int CreatedUserId);
        Task<Result<bool>> DeleteCategory(int id, int CreatedUserId);
    }

}
