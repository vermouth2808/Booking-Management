using AutoMapper;
using Core.Domain.Entities;
using Core.Infrastructure.Redis;
using Core.Infrastructure.Repositories.Interfaces;
using Core.Shared.Common.Models;
using Core.Shared.DTOs.Request.Room;
using Core.Shared.DTOs.Response.Room;
using Microsoft.EntityFrameworkCore;
using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Dapper;

namespace Core.Infrastructure.Repositories
{
    public class RoomRepository<T> : IRoomRepository<T> where T : RoomRes, new()
    {
        private readonly string _connectionString;
        private readonly BookMovieTicketContext _context;
        private readonly IMapper _mapper;
        private readonly IRedisCacheService _redisCacheService;

        public RoomRepository(IConfiguration configuration, BookMovieTicketContext context, IMapper mapper, IRedisCacheService redisCacheService)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
            _context = context;
            _mapper = mapper;
            _redisCacheService = redisCacheService;
        }

        public async Task<Result<bool>> CreateRoom(CreateRoomReq req, int createdUserId)
        {
            var Room = new Room()
            {
                RoomName = req.RoomName,
                Layout = req.Layout,
                Description = req.Description,
                CreatedDate = DateTime.UtcNow,
                CreatedUserId = createdUserId,
                UpdatedDate = DateTime.UtcNow,
                UpdatedUserId = createdUserId,
                IsDeleted = false,
            };
            _context.Rooms.Add(Room);
            await _context.SaveChangesAsync();
            await _redisCacheService.RemoveByPatternAsync("Rooms_");
            return Result<bool>.Success(true);
        }

        public async Task<Result<bool>> DeleteRoom(int id, int updatedUserId)
        {
            string cacheKey = $"Room_{id}";
            var Room = await _context.Rooms.FirstOrDefaultAsync(s => s.RoomId == id && s.IsDeleted == false);
            if (Room == null)
            {
                return Result<bool>.Failure("Room not found");
            }

            Room.IsDeleted = true;
            Room.UpdatedDate = DateTime.UtcNow;
            Room.UpdatedUserId = updatedUserId;

            _context.Rooms.Update(Room);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("Rooms_");
            await _redisCacheService.RemoveDataAsync(cacheKey);

            return Result<bool>.Success(true);
        }

        public async Task<Result<T>> GetRoomById(int id)
        {
            string cacheKey = $"Room_{id}";

            // Kiểm tra cache trước
            var cachedRoom = await _redisCacheService.GetDataAsync<RoomRes>(cacheKey);
            if (cachedRoom != null)
            {
                return Result<T>.Success(_mapper.Map<T>(cachedRoom), "Successfully");
            }

            using (var connection = new SqlConnection(_connectionString))
            {
                var parameters = new { RoomID = id };
                var room = await connection.QueryFirstOrDefaultAsync<RoomRes>(
                    "Room_Read_By_ID",
                    parameters,
                    commandType: CommandType.StoredProcedure
                );

                if (room == null)
                {
                    return Result<T>.Failure("Room not found");
                }

                await _redisCacheService.SetDataAsync(cacheKey, room, null);

                return Result<T>.Success(_mapper.Map<T>(room), "Successfully");
            }
        }


        /*     public async Task<Result<RoomSearchRes>> SearchRoom(SearchRoomReq req)
             {
                 string processedKeySearch = req.KeySearch?.Trim();
                 processedKeySearch = Regex.Replace(processedKeySearch, "\\s+", " ");
                 string cacheKey = $"Rooms_{req.KeySearch}_{req.PageIndex}_{req.PageSize}";

                 var cachedRooms = await _redisCacheService.GetDataAsync<RoomSearchRes>(cacheKey);
                 if (cachedRooms != null)
                 {
                     return Result<RoomSearchRes>.Success(cachedRooms, "Successfully retrieved from cache");
                 }

                 var query = _context.Rooms.AsQueryable().Where(s => s.IsDeleted == false);

                 *//*  if (!string.IsNullOrEmpty(req.KeySearch))
                   {
                       query = query.Where(s => s.RoomId.Contains(req.KeySearch));
                   }
       *//*
                 int totalRecords = await query.CountAsync();
                 var pageIndex = Math.Max(1, req.PageIndex);
                 var fromDate = DateOnly.FromDateTime(req.FromDate);
                 var toDate = DateOnly.FromDateTime(req.ToDate);
                 var Rooms = await query
                     .Where(s => !s.IsDeleted
                         && (!req.MovieId.HasValue || s.MovieId == req.MovieId)
                         && (!req.RoomId.HasValue || s.RoomId == req.RoomId)
                         && DateOnly.FromDateTime((DateTime)s.StartTime) >= fromDate
                         && DateOnly.FromDateTime((DateTime)s.StartTime) <= toDate)
                     .OrderBy(s => s.CreatedDate)
                     .Skip((pageIndex - 1) * req.PageSize)
                     .Take(req.PageSize)
                     .Include(s => s.Movie)
                     .Include(s => s.Room)
                     .Select(s => new RoomRes
                     {
                         RoomId = s.RoomId,
                         MovieId = s.MovieId,
                         RoomId = s.RoomId,
                         StartTime = s.StartTime,
                         EndTime = s.EndTime,
                         Price = s.Price,
                         Title = s.Movie.Title,
                         Director = s.Movie.Director,
                         Performer = s.Movie.Performer,
                         Language = s.Movie.Language,
                         Genre = s.Movie.Genre,
                         Duration = s.Movie.Duration ?? 0,
                         ReleaseDate = s.Movie.ReleaseDate.Value.ToDateTime(TimeOnly.MinValue),
                         PosterUrl = s.Movie.PosterUrl,
                         TrailerUrl = s.Movie.TrailerUrl,
                         AgeRating = s.Movie.AgeRating,
                         RoomName = s.Room.RoomName
                     })
                     .ToListAsync();


                 if (!Rooms.Any())
                 {
                     return Result<RoomSearchRes>.Failure("No Rooms found");
                 }

                 var mappedRooms = Rooms.Select(_mapper.Map<T>);
                 var searchResult = new RoomSearchRes
                 {
                     TotalRecords = totalRecords,
                     Rooms = mappedRooms
                 };
                 _redisCacheService.SetDataAsync(cacheKey, mappedRooms, null);

                 return Result<RoomSearchRes>.Success(searchResult, "Successfully retrieved from database");
             }*/

        public async Task<Result<bool>> UpdateRoom(UpdateRoomReq req, int updatedUserId)
        {
            var Room = await _context.Rooms.FirstOrDefaultAsync(s => s.RoomId == req.RoomId && s.IsDeleted == false);
            if (Room == null)
            {
                return Result<bool>.Failure("Room not found");
            }
            string cacheKey = $"Room_{req.RoomId}";
            Room.RoomName = req.RoomName ?? Room.RoomName;
            Room.Layout = req.Layout ?? Room.Layout;
            Room.Description = req.Description ?? Room.Description;
            _context.Rooms.Update(Room);
            await _context.SaveChangesAsync();

            await _redisCacheService.RemoveByPatternAsync("Rooms_");
            await _redisCacheService.RemoveDataAsync(cacheKey);
            return Result<bool>.Success(true);
        }
    }
}