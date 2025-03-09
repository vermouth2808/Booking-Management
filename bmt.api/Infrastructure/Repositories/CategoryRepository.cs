using AutoMapper;
using Core.Domain.Entities;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Category;
using Core.Shared.DTOs.Response.Category;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories
{
    public class CategoryRepository<T> : ICategoryRepository<T> where T : CategoryRes, new()
    {
        private readonly BookMovieTicketContext _context;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public CategoryRepository(BookMovieTicketContext context, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _context = context;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        public async Task<Result<bool>> CreateCategory(CreateCategoryReq req, int CreatedUserId)
        {
            var Category = new Category()
            {
                CategoryName = req.CategoryName,
                Description = req.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = CreatedUserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = CreatedUserId,
                IsDeleted = false,
            };
            _context.Categories.Add(Category);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveByPatternAsync("Categorys_");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteCategory(int id, int updatedUserId)
        {
            string cacheKey = $"Category_{id}";
            var Category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id && m.IsDeleted == false);
            if (Category == null)
            {
                return Result<bool>.Failure("Category not found");
            }

            Category.IsDeleted = true;
            Category.UpdatedDate = DateTime.UtcNow;
            Category.UpdatedUserId = updatedUserId;

            _context.Categories.Update(Category);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("Categorys_");
            await _redisCacheService.RemoveDataAsync(cacheKey);

            return Result<bool>.Success(true);
        }


        public async Task<Result<T>> GetCategoryById(int id)
        {
            string cacheKey = $"Category_{id}";

            var cachedCategory = await _redisCacheService.GetDataAsync<Category>(cacheKey);
            if (cachedCategory != null)
            {
                var mappedCategory = _mapper.Map<T>(cachedCategory);
                return Result<T>.Success(mappedCategory, "Successfully");
            }

            var efItem = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == id && m.IsDeleted == false);
            if (efItem == null)
            {
                return Result<T>.Failure("Category not found");
            }

            _redisCacheService.SetDataAsync(cacheKey, efItem, null);

            var mappedResult = _mapper.Map<T>(efItem);
            return Result<T>.Success(mappedResult, "Successfully");
        }

        public async Task<Result<IEnumerable<T>>> ReadAllCategory()
        {

            var cachedCategorys = await _redisCacheService.GetDataAsync<IEnumerable<T>>("Categorys");
            if (cachedCategorys != null)
            {
                return Result<IEnumerable<T>>.Success(cachedCategorys, "Successfully retrieved from cache");
            }

            var query = _context.Categories.Where(m => m.IsDeleted == false);

            if (!query.Any())
            {
                return Result<IEnumerable<T>>.Failure("No Categorys found");
            }

            // Mapping dữ liệu trước khi lưu cache & trả về
            var mappedCategorys = query.Select(_mapper.Map<T>);
            _redisCacheService.SetDataAsync("Categorys", mappedCategorys, null);

            return Result<IEnumerable<T>>.Success(mappedCategorys, "Successfully retrieved from database");
        }

        public async Task<Result<bool>> UpdateCategory(UpdateCategoryReq req, int updatedUserId)
        {
            var Category = await _context.Categories.FirstOrDefaultAsync(m => m.CategoryId == req.CategoryId && m.IsDeleted == false);
            if (Category == null)
            {
                return Result<bool>.Failure("Category not found");
            }
            string cacheKey = $"Category_{req.CategoryId}";
            Category.CategoryName = req.CategoryName ?? Category.CategoryName;
            Category.Description = req.Description ?? Category.Description;
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("Categorys_");
            await _redisCacheService.RemoveByPatternAsync("Categorys");
            await _redisCacheService.RemoveDataAsync(cacheKey);
            return Result<bool>.Success(true);
        }
    }
}
