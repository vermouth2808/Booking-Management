using Core.Domain.Entities;
using Core.Infrastructure.Mappings;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Banner;
using Core.Shared.DTOs.Response.Banner;
using Microsoft.EntityFrameworkCore;

namespace Core.Infrastructure.Repositories
{
    public class BannerRepository<T> : IBannerRepository<T> where T : BannerRes, new()
    {
        private readonly BookMovieTicketContext _context;
        private readonly IBannerMapper<T> _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public BannerRepository(BookMovieTicketContext context, IBannerMapper<T> mapper, IRedisCacheService redisCacheService)
        {
            _context = context;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        public async Task<Result<bool>> CreateBanner(CreateBannerReq req, int CreatedUserId)
        {
            var Banner = new Banner()
            {
                BannerName = req.BannerName,
                BannerUrl = req.BannerUrl,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = CreatedUserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = CreatedUserId,
                IsDeleted = false,
            };
            _context.Banners.Add(Banner);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveByPatternAsync("Banner_");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteBanner(int id, int updatedUserId)
        {
            string cacheKey = $"Banner_{id}";
            var Banner = await _context.Banners.FirstOrDefaultAsync(m => m.BannerId == id && m.IsDeleted == false);
            if (Banner == null)
            {
                return Result<bool>.Failure("Banner not found");
            }

            Banner.IsDeleted = true;
            Banner.UpdatedDate = DateTime.UtcNow;
            Banner.UpdatedUserId = updatedUserId;

            _context.Banners.Update(Banner);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("Banner_");
            await _redisCacheService.RemoveDataAsync(cacheKey);

            return Result<bool>.Success(true);
        }


        public async Task<Result<T>> GetBannerById(int id)
        {
            string cacheKey = $"Banner_{id}";

            var cachedBanner = await _redisCacheService.GetDataAsync<Banner>(cacheKey);
            if (cachedBanner != null)
            {
                var mappedBanner = _mapper.ToModel(cachedBanner);
                return Result<T>.Success(mappedBanner, "Successfully");
            }

            var efItem = await _context.Banners.FirstOrDefaultAsync(m => m.BannerId == id && m.IsDeleted == false);
            if (efItem == null)
            {
                return Result<T>.Failure("Banner not found");
            }

            _redisCacheService.SetDataAsync(cacheKey, efItem, null);

            var mappedResult = _mapper.ToModel(efItem);
            return Result<T>.Success(mappedResult, "Successfully");
        }

        public async Task<Result<ReadAllBannerRes>> ReadAllBanner()
        {

            var cachedBanners = await _redisCacheService.GetDataAsync<ReadAllBannerRes>("Banner");
            if (cachedBanners != null)
            {
                return Result<ReadAllBannerRes>.Success(cachedBanners, "Successfully retrieved from cache");
            }

            var Banners = _context.Banners.AsQueryable().Where(m => m.IsDeleted == false);

            int totalRecords = await Banners.CountAsync();

            if (!Banners.Any())
            {
                return Result<ReadAllBannerRes>.Failure("No Banners found");
            }

            // Mapping dữ liệu trước khi lưu cache & trả về
            var mappedBanners = Banners.Select(_mapper.ToModel);
            var Result = new ReadAllBannerRes
            {
                TotalRecords = totalRecords,
                Banners = mappedBanners
            };
            _redisCacheService.SetDataAsync("Banner", mappedBanners, null);

            return Result<ReadAllBannerRes>.Success(Result, "Successfully retrieved from database");
        }

        public async Task<Result<bool>> UpdateBanner(UpdateBannerReq req, int updatedUserId)
        {
            string cacheKey = $"Banner_{req.BannerId}";
            var Banner = await _context.Banners.FirstOrDefaultAsync(m => m.BannerId == req.BannerId && m.IsDeleted == false);
            if (Banner == null)
            {
                return Result<bool>.Failure("Banner not found");
            }
            Banner.BannerName = req.BannerName ?? Banner.BannerName;
            Banner.BannerUrl = req.BannerUrl ?? Banner.BannerUrl;
            _context.Banners.Update(Banner);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("Banner_");
            await _redisCacheService.RemoveDataAsync(cacheKey);
            await _redisCacheService.RemoveDataAsync("Banner");
            return Result<bool>.Success(true);
        }
    }
}
