using AutoMapper;
using Core.Domain.Entities;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.FoodOrder;
using Core.Shared.DTOs.Response.FoodOrder;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Core.Infrastructure.Repositories
{
    public class FoodOrderRepository<T> : IFoodOrderRepository<T> where T : FoodOrderRes, new()
    {
        private readonly BookMovieTicketContext _context;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public FoodOrderRepository(BookMovieTicketContext context, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _context = context;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        public async Task<Result<bool>> CreateFoodOrder(CreateFoodOrderReq req, int CreatedUserId)
        {
            var FoodOrder = new FoodOrder()
            {
                BookingId = req.BookingId,
                ComboId = req.ComboId,
                Quantity = req.Quantity,
                TotalPrice = req.TotalPrice,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = CreatedUserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = CreatedUserId,
                IsDeleted = false,
            };

            _context.FoodOrders.Add(FoodOrder);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveByPatternAsync("FoodOrders_");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteFoodOrder(int id, int updatedUserId)
        {
            string cacheKey = $"FoodOrder_{id}";
            var FoodOrder = await _context.FoodOrders.FirstOrDefaultAsync(m => m.FoodOrderId == id && m.IsDeleted == false);
            if (FoodOrder == null)
            {
                return Result<bool>.Failure("FoodOrder not found");
            }

            FoodOrder.IsDeleted = true;
            FoodOrder.UpdatedDate = DateTime.UtcNow;
            FoodOrder.UpdatedUserId = updatedUserId;

            _context.FoodOrders.Update(FoodOrder);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("FoodOrders_");
            await _redisCacheService.RemoveDataAsync(cacheKey);

            return Result<bool>.Success(true);
        }


        public async Task<Result<T>> GetFoodOrderById(int id)
        {
            string cacheKey = $"FoodOrder_{id}";

            var cachedFoodOrder = await _redisCacheService.GetDataAsync<FoodOrder>(cacheKey);
            if (cachedFoodOrder != null)
            {
                var mappedFoodOrder = _mapper.Map<T>(cachedFoodOrder);
                return Result<T>.Success(mappedFoodOrder, "Successfully");
            }

            var efItem = await _context.FoodOrders.FirstOrDefaultAsync(m => m.FoodOrderId == id && m.IsDeleted == false);
            if (efItem == null)
            {
                return Result<T>.Failure("FoodOrder not found");
            }

            _redisCacheService.SetDataAsync(cacheKey, efItem, null);

            var mappedResult = _mapper.Map<T>(efItem);
            return Result<T>.Success(mappedResult, "Successfully");
        }

        public async Task<Result<FoodOrderSearchRes>> SearchFoodOrder(SearchFoodOrderReq req)
        {
            string processedKeySearch = req.KeySearch?.Trim();
            processedKeySearch = Regex.Replace(processedKeySearch, @"\s+", " ");
            string cacheKey = $"FoodOrders_{req.KeySearch}_{req.PageIndex}_{req.PageSize}";

            var cachedFoodOrders = await _redisCacheService.GetDataAsync<FoodOrderSearchRes>(cacheKey);
            if (cachedFoodOrders != null)
            {
                return Result<FoodOrderSearchRes>.Success(cachedFoodOrders, "Successfully retrieved from cache");
            }

            var query = _context.FoodOrders.AsQueryable().Where(m => m.IsDeleted == false);

            /*if (!string.IsNullOrEmpty(req.KeySearch))
            {
                query = query.Where(m => m.Title.Contains(req.KeySearch));
            }*/

            int totalRecords = await query.CountAsync();
            var fromDate = DateOnly.FromDateTime(req.FromDate);
            var toDate = DateOnly.FromDateTime(req.ToDate);

            var pageIndex = Math.Max(1, req.PageIndex);
            var FoodOrders = await query
                .Where(m => (bool)!m.IsDeleted)
                .OrderBy(m => m.CreatedDate)
                .Skip((pageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToListAsync();



            if (!FoodOrders.Any())
            {
                return Result<FoodOrderSearchRes>.Failure("No FoodOrders found");
            }

            // Mapping dữ liệu trước khi lưu cache & trả về
            var mappedFoodOrders = FoodOrders.Select(_mapper.Map<T>);
            var searchResult = new FoodOrderSearchRes
            {
                TotalRecords = totalRecords,
                FoodOrders = mappedFoodOrders
            };
            _redisCacheService.SetDataAsync(cacheKey, mappedFoodOrders, null);

            return Result<FoodOrderSearchRes>.Success(searchResult, "Successfully retrieved from database");
        }

        public async Task<Result<bool>> UpdateFoodOrder(UpdateFoodOrderReq req, int updatedUserId)
        {
            var FoodOrder = await _context.FoodOrders.FirstOrDefaultAsync(m => m.FoodOrderId == req.FoodOrderId && m.IsDeleted == false);
            if (FoodOrder == null)
            {
                return Result<bool>.Failure("FoodOrder not found");
            }
            string cacheKey = $"FoodOrder_{req.FoodOrderId}";
            FoodOrder.BookingId = req.BookingId ?? FoodOrder.BookingId;
            FoodOrder.ComboId = req.ComboId ?? FoodOrder.ComboId;
            FoodOrder.Quantity = req.Quantity != 0 ? req.Quantity : FoodOrder.Quantity;
            FoodOrder.TotalPrice = req.TotalPrice != 0 ? req.TotalPrice : FoodOrder.TotalPrice;

            _context.FoodOrders.Update(FoodOrder);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("FoodOrders_");
            await _redisCacheService.RemoveDataAsync(cacheKey);
            return Result<bool>.Success(true);
        }
    }
}
