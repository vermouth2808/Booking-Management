using Core.Application.Interfaces;
using Core.Shared.Common.Models;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Response.FoodCombo;
using Core.Shared.DTOs.Request.FoodCombo;

namespace Core.Application.Services
{
    public class FoodComboService<T> : IFoodComboService<T> where T : FoodComboRes, new()
    {
        private readonly IFoodComboRepository<T> _FoodComboRepository;

        public FoodComboService(IFoodComboRepository<T> FoodComboRepository)
        {
            _FoodComboRepository = FoodComboRepository;
        }
        /// <summary>
        /// read FoodCombo by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<T>> GetFoodComboById(int id)
        {
            var result = await _FoodComboRepository.GetFoodComboById(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return Result<T>.Failure("FoodCombo not found");
            }

            return Result<T>.Success(result.Data);
        }


        /// <summary>
        /// seach FoodCombo
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<IEnumerable<T>>> ReadAllFoodCombo()
        {
            var result = await _FoodComboRepository.ReadAllFoodCombo();
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<IEnumerable<T>>.Failure("No FoodCombos found");
            }

            return Result<IEnumerable<T>>.Success(result.Data);
        }
        /// <summary>
        /// create a FoodCombo
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<bool>> CreateFoodCombo(CreateFoodComboReq req, int CreatedUserId)
        {
            var result = await _FoodComboRepository.CreateFoodCombo(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Create a failed FoodCombo");
            }

            return Result<bool>.Success(true);
        }
        /// <summary>
        /// update a FoodCombo
        /// </summary>
        /// <param name="req"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> UpdateFoodCombo(UpdateFoodComboReq req, int CreatedUserId)
        {
            var result = await _FoodComboRepository.UpdateFoodCombo(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed FoodCombo");
            }
            return Result<bool>.Success(true);
        }
        /// <summary>
        /// remove a FoodCombo
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteFoodCombo(int id, int CreatedUserId)
        {
            var result = await _FoodComboRepository.DeleteFoodCombo(id, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed FoodCombo");
            }
            return Result<bool>.Success(true);
        }
    }
}
