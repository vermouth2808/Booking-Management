using Core.Application.Interfaces;
using Core.Shared.Common.Models;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.DTOs.Response.FoodOrder;
using Core.Shared.DTOs.Request.FoodOrder;

namespace Core.Application.Services
{
    public class FoodOrderService<T> : IFoodOrderService<T> where T : FoodOrderRes, new()
    {
        private readonly IFoodOrderRepository<T> _FoodOrderRepository;

        public FoodOrderService(IFoodOrderRepository<T> FoodOrderRepository)
        {
            _FoodOrderRepository = FoodOrderRepository;
        }
        /// <summary>
        /// read FoodOrder by id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Result<T>> GetFoodOrderById(int id)
        {
            var result = await _FoodOrderRepository.GetFoodOrderById(id);

            if (!result.IsSuccess || result.Data == null)
            {
                return Result<T>.Failure("FoodOrder not found");
            }

            return Result<T>.Success(result.Data);
        }


        /// <summary>
        /// seach FoodOrder
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<FoodOrderSearchRes>> SearchFoodOrder(SearchFoodOrderReq req)
        {
            var result = await _FoodOrderRepository.SearchFoodOrder(req);
            if (!result.IsSuccess || result.Data == null)
            {
                return Result<FoodOrderSearchRes>.Failure("No FoodOrders found");
            }

            return Result<FoodOrderSearchRes>.Success(result.Data);
        }
        /// <summary>
        /// create a FoodOrder
        /// </summary>
        /// <param name="req"></param>
        /// <returns></returns>
        public async Task<Result<bool>> CreateFoodOrder(CreateFoodOrderReq req, int CreatedUserId)
        {
            var result = await _FoodOrderRepository.CreateFoodOrder(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Create a failed FoodOrder");
            }

            return Result<bool>.Success(true);
        }
        /// <summary>
        /// update a FoodOrder
        /// </summary>
        /// <param name="req"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> UpdateFoodOrder(UpdateFoodOrderReq req, int CreatedUserId)
        {
            var result = await _FoodOrderRepository.UpdateFoodOrder(req, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed FoodOrder");
            }
            return Result<bool>.Success(true);
        }
        /// <summary>
        /// remove a FoodOrder
        /// </summary>
        /// <param name="id"></param>
        /// <param name="CreatedUserId"></param>
        /// <returns></returns>
        public async Task<Result<bool>> DeleteFoodOrder(int id, int CreatedUserId)
        {
            var result = await _FoodOrderRepository.DeleteFoodOrder(id, CreatedUserId);
            if (!result.IsSuccess)
            {
                return Result<bool>.Failure("Update a failed FoodOrder");
            }
            return Result<bool>.Success(true);
        }
    }
}
