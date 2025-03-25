using AutoMapper;
using Core.Domain.Entities;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.FoodCombo;
using Core.Shared.DTOs.Response.FoodCombo;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories
{
    public class FoodComboRepository<T> : IFoodComboRepository<T> where T : FoodComboRes, new()
    {
        private readonly BookMovieTicketContext _context;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public FoodComboRepository(BookMovieTicketContext context, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _context = context;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        public async Task<Result<bool>> CreateFoodCombo(CreateFoodComboReq req, int CreatedUserId)
        {
            var FoodCombo = new FoodCombo()
            {
                Name = req.Name,
                Price = req.Price,
                ImageUrl = req.ImageUrl,
                Description = req.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = CreatedUserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = CreatedUserId,
                IsDeleted = false,
            };
            _context.FoodCombos.Add(FoodCombo);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveByPatternAsync("FoodCombos_");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteFoodCombo(int id, int updatedUserId)
        {
            string cacheKey = $"FoodCombo_{id}";
            var FoodCombo = await _context.FoodCombos.FirstOrDefaultAsync(m => m.ComboId == id && m.IsDeleted == false);
            if (FoodCombo == null)
            {
                return Result<bool>.Failure("FoodCombo not found");
            }

            FoodCombo.IsDeleted = true;
            FoodCombo.UpdatedDate = DateTime.UtcNow;
            FoodCombo.UpdatedUserId = updatedUserId;

            _context.FoodCombos.Update(FoodCombo);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("FoodCombos_");
            await _redisCacheService.RemoveDataAsync(cacheKey);

            return Result<bool>.Success(true);
        }


        public async Task<Result<T>> GetFoodComboById(int id)
        {
            string cacheKey = $"FoodCombo_{id}";

            var cachedFoodCombo = await _redisCacheService.GetDataAsync<FoodCombo>(cacheKey);
            if (cachedFoodCombo != null)
            {
                var mappedFoodCombo = _mapper.Map<T>(cachedFoodCombo);
                return Result<T>.Success(mappedFoodCombo, "Successfully");
            }

            var efItem = await _context.FoodCombos.FirstOrDefaultAsync(m => m.ComboId == id && m.IsDeleted == false);
            if (efItem == null)
            {
                return Result<T>.Failure("FoodCombo not found");
            }

            _redisCacheService.SetDataAsync(cacheKey, efItem, null);

            var mappedResult = _mapper.Map<T>(efItem);
            return Result<T>.Success(mappedResult, "Successfully");
        }

        public async Task<Result<IEnumerable<T>>> ReadAllFoodCombo()
        {

            var cachedFoodCombos = await _redisCacheService.GetDataAsync<IEnumerable<T>>("FoodCombos");
            if (cachedFoodCombos != null)
            {
                return Result<IEnumerable<T>>.Success(cachedFoodCombos, "Successfully retrieved from cache");
            }

            var query = _context.FoodCombos.Where(m => m.IsDeleted == false);

            if (!query.Any())
            {
                return Result<IEnumerable<T>>.Failure("No FoodCombos found");
            }

            // Mapping dữ liệu trước khi lưu cache & trả về
            var mappedFoodCombos = query.Select(_mapper.Map<T>);
            _redisCacheService.SetDataAsync("FoodCombos", mappedFoodCombos, null);

            return Result<IEnumerable<T>>.Success(mappedFoodCombos, "Successfully retrieved from database");
        }

        public async Task<Result<bool>> UpdateFoodCombo(UpdateFoodComboReq req, int updatedUserId)
        {
            var FoodCombo = await _context.FoodCombos.FirstOrDefaultAsync(m => m.ComboId == req.ComboId && m.IsDeleted == false);
            if (FoodCombo == null)
            {
                return Result<bool>.Failure("FoodCombo not found");
            }
            string cacheKey = $"FoodCombo_{req.ComboId}";
            FoodCombo.Name = req.Name ?? FoodCombo.Name;
            FoodCombo.Description = req.Description ?? FoodCombo.Description;
            FoodCombo.Price = req.Price != 0 ? req.Price : FoodCombo.Price;
            FoodCombo.ImageUrl = req.ImageUrl ?? FoodCombo.ImageUrl;
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("FoodCombos_");
            await _redisCacheService.RemoveByPatternAsync("FoodCombos");
            await _redisCacheService.RemoveDataAsync(cacheKey);
            return Result<bool>.Success(true);
        }
    }
}
