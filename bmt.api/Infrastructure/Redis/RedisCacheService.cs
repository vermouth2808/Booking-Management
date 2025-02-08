using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;
using System.Text.Json;

namespace Core.Infrastructure.Redis
{
    public class RedisCacheService : IRedisCacheService
    {
        private readonly IDistributedCache _cache;
        private readonly IConnectionMultiplexer _redis;

        public RedisCacheService(IDistributedCache cache, IConnectionMultiplexer redis)
        {
            _cache = cache ?? throw new ArgumentNullException(nameof(cache));
            _redis = redis ?? throw new ArgumentNullException(nameof(redis));
        }

        /// <summary>
        /// Lấy dữ liệu từ Redis
        /// </summary>
        public async Task<T?> GetDataAsync<T>(string key)
        {
            try
            {
                var data = await _cache.GetStringAsync(key);
                if (string.IsNullOrEmpty(data))
                {
                    Console.WriteLine($"[Redis] Cache MISS: '{key}' không có trong Redis.");
                    return default;
                }

                Console.WriteLine($"[Redis] Cache HIT: '{key}', Data: {data}");

                return JsonSerializer.Deserialize<T>(data, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase
                });
            }
            catch (JsonException ex)
            {
                Console.WriteLine($"[Redis] Lỗi JSON khi deserialize key '{key}': {ex.Message}");
                return default;
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"[Redis] Lỗi Redis khi lấy dữ liệu key '{key}': {ex.Message}");
                return default;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Lỗi không xác định khi lấy key '{key}': {ex.Message}");
                return default;
            }
        }

        /// <summary>
        /// Lưu dữ liệu vào Redis với thời gian hết hạn mặc định 30 phút
        /// </summary>
        public async Task SetDataAsync<T>(string key, T value, TimeSpan? expirationTime = null)
        {
            try
            {
                if (value == null)
                {
                    Console.WriteLine($"[Redis] Cảnh báo: Cố gắng cache NULL cho key '{key}'");
                    return;
                }

                var jsonData = JsonSerializer.Serialize(value, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull
                });

                var options = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = expirationTime ?? TimeSpan.FromMinutes(30)
                };

                await _cache.SetStringAsync(key, jsonData, options);
                Console.WriteLine($"[Redis] Đã cache key '{key}' với TTL {(expirationTime?.TotalMinutes ?? 30)} phút.");
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"[Redis] Lỗi Redis khi lưu key '{key}': {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Lỗi không xác định khi lưu key '{key}': {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa dữ liệu cache theo key cụ thể
        /// </summary>
        public async Task RemoveDataAsync(string key)
        {
            try
            {
                await _cache.RemoveAsync(key);
                Console.WriteLine($"[Redis] Đã xóa cache key '{key}'");
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"[Redis] Lỗi Redis khi xóa key '{key}': {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Lỗi không xác định khi xóa key '{key}': {ex.Message}");
            }
        }

        /// <summary>
        /// Xóa cache theo pattern (vd: 'movies_*')
        /// </summary>
        public async Task RemoveByPatternAsync(string pattern)
        {
            try
            {
                var endpoints = _redis.GetEndPoints();
                if (!endpoints.Any())
                {
                    Console.WriteLine($"[Redis] Không tìm thấy server nào để xóa pattern '{pattern}'");
                    return;
                }

                var server = _redis.GetServer(endpoints.First());
                var keys = server.Keys(pattern: $"{pattern}*").ToList();

                if (!keys.Any())
                {
                    Console.WriteLine($"[Redis] Không tìm thấy key nào phù hợp với pattern '{pattern}'");
                    return;
                }

                foreach (var key in keys)
                {
                    await _cache.RemoveAsync(key);
                    Console.WriteLine($"[Redis] Đã xóa cache key '{key}' theo pattern '{pattern}'");
                }
            }
            catch (RedisException ex)
            {
                Console.WriteLine($"[Redis] Lỗi Redis khi xóa cache theo pattern '{pattern}': {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[Redis] Lỗi không xác định khi xóa cache theo pattern '{pattern}': {ex.Message}");
            }
        }
    }
}
