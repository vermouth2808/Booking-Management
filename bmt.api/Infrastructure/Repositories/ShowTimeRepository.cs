using AutoMapper;
using Core.Domain.Entities;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.ShowTime;
using Core.Shared.DTOs.Response.ShowTime;
using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;

namespace Core.Infrastructure.Repositories
{
    public class ShowTimeRepository<T> : IShowTimeRepository<T> where T : ShowTimeRes, new()
    {
        private readonly BookMovieTicketContext _context;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public ShowTimeRepository(BookMovieTicketContext context, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _context = context;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        public async Task<Result<bool>> CreateShowTime(CreateShowTimeReq req, int createdUserId)
        {
            var ShowTime = new Showtime()
            {
                RoomId = req.RoomId,
                StartTime = req.StartTime,
                EndTime = req.EndTime,
                Price = req.Price,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = createdUserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = createdUserId,
                IsDeleted = false,
            };
            _context.Showtimes.Add(ShowTime);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveByPatternAsync("ShowTimes_");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteShowTime(int id, int updatedUserId)
        {
            string cacheKey = $"ShowTime_{id}";
            var ShowTime = await _context.Showtimes.FirstOrDefaultAsync(s => s.ShowtimeId == id && s.IsDeleted == false);
            if (ShowTime == null)
            {
                return Result<bool>.Failure("ShowTime not found");
            }

            ShowTime.IsDeleted = true;
            ShowTime.UpdatedDate = DateTime.UtcNow;
            ShowTime.UpdatedUserId = updatedUserId;

            _context.Showtimes.Update(ShowTime);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("ShowTimes_");
            await _redisCacheService.RemoveDataAsync(cacheKey);

            return Result<bool>.Success(true);
        }

        public async Task<Result<T>> GetShowTimeById(int id)
        {
            string cacheKey = $"ShowTime_{id}";

            var cachedShowTime = await _redisCacheService.GetDataAsync<Showtime>(cacheKey);
            if (cachedShowTime != null)
            {
                var mappedShowTime = _mapper.Map<T>(cachedShowTime);
                return Result<T>.Success(mappedShowTime, "Successfully");
            }

            var efItem = await _context.Showtimes.FirstOrDefaultAsync(s => s.ShowtimeId == id && s.IsDeleted == false);
            if (efItem == null)
            {
                return Result<T>.Failure("ShowTime not found");
            }

            _redisCacheService.SetDataAsync(cacheKey, efItem, null);

            var mappedResult = _mapper.Map<T>(efItem);
            return Result<T>.Success(mappedResult, "Successfully");
        }

        public async Task<Result<ShowTimeSearchRes>> SearchShowTime(SearchShowTimeReq req)
        {
            string processedKeySearch = req.KeySearch?.Trim();
            processedKeySearch = Regex.Replace(processedKeySearch, "\\s+", " ");
            string cacheKey = $"ShowTimes_{req.KeySearch}_{req.PageIndex}_{req.PageSize}";

            var cachedShowTimes = await _redisCacheService.GetDataAsync<ShowTimeSearchRes>(cacheKey);
            if (cachedShowTimes != null)
            {
                return Result<ShowTimeSearchRes>.Success(cachedShowTimes, "Successfully retrieved from cache");
            }

            var query = _context.Showtimes.AsQueryable().Where(s => s.IsDeleted == false);

          /*  if (!string.IsNullOrEmpty(req.KeySearch))
            {
                query = query.Where(s => s.RoomId.Contains(req.KeySearch));
            }
*/
            int totalRecords = await query.CountAsync();

            var ShowTimes = await query.OrderBy(s => s.CreatedDate)
                .Skip((req.PageIndex - 1) * req.PageSize)
                .Take(req.PageSize)
            .ToListAsync();

            if (!ShowTimes.Any())
            {
                return Result<ShowTimeSearchRes>.Failure("No ShowTimes found");
            }

            var mappedShowTimes = ShowTimes.Select(_mapper.Map<T>);
            var searchResult = new ShowTimeSearchRes
            {
                TotalRecords = totalRecords,
                ShowTimes = mappedShowTimes
            };
            _redisCacheService.SetDataAsync(cacheKey, mappedShowTimes, null);

            return Result<ShowTimeSearchRes>.Success(searchResult, "Successfully retrieved from database");
        }

        public async Task<Result<bool>> UpdateShowTime(UpdateShowTimeReq req, int updatedUserId)
        {
            var ShowTime = await _context.Showtimes.FirstOrDefaultAsync(s => s.ShowtimeId == req.ShowtimeId && s.IsDeleted == false);
            if (ShowTime == null)
            {
                return Result<bool>.Failure("ShowTime not found");
            }
            string cacheKey = $"ShowTime_{req.ShowtimeId}";
            //ShowTime.RoomId = req.RoomId ?? ShowTime.RoomId;
            ShowTime.StartTime = req.StartTime ?? ShowTime.StartTime;
            ShowTime.EndTime = req.EndTime ?? ShowTime.EndTime;
            ShowTime.Price = req.Price ?? ShowTime.Price;
            _context.Showtimes.Update(ShowTime);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("ShowTimes_");
            await _redisCacheService.RemoveDataAsync(cacheKey);
            return Result<bool>.Success(true);
        }
    }
}