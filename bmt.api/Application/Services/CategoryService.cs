using Core.Application.Interfaces;
using Core.Shared.Common.Models;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Response.Category;
using Core.Shared.DTOs.Request.Category;

namespace Core.Application.Services
{
    public class CategoryService<T> : ICategoryService<T> where T : CategoryRes, new()
    {
        private readonly ICategoryRepository<T> _CategoryRepository;

        public CategoryService(ICategoryRepository<T> CategoryRepository)
        {
            _CategoryRepository = CategoryRepository;
        }
        /// <summary>
        /// read Category by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<T>> GetCategoryById(int id)
        {
            var result = await _CategoryRepository.GetCategoryById(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return Result<T>.Failure("Category not found");
            }

            return Result<T>.Success(result.Data);
        }


        /// <summary>
        /// seach Category
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<IEnumerable<T>>> ReadAllCategory()
        {
            var result = await _CategoryRepository.ReadAllCategory();
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<IEnumerable<T>>.Failure("No Categorys found");
            }

            return Result<IEnumerable<T>>.Success(result.Data);
        }
        /// <summary>
        /// create a Category
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<bool>> CreateCategory(CreateCategoryReq req, int CreatedUserId)
        {
            var result = await _CategoryRepository.CreateCategory(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Create a failed Category");
            }

            return Result<bool>.Success(true);
        }
        /// <summary>
        /// update a Category
        /// </summary>
        /// <param name="req"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> UpdateCategory(UpdateCategoryReq req, int CreatedUserId)
        {
            var result = await _CategoryRepository.UpdateCategory(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed Category");
            }
            return Result<bool>.Success(true);
        }
        /// <summary>
        /// remove a Category
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteCategory(int id, int CreatedUserId)
        {
            var result = await _CategoryRepository.DeleteCategory(id, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed Category");
            }
            return Result<bool>.Success(true);
        }
    }
}
