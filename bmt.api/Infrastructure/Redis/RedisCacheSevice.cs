using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Core.Infrastructure.Redis
{
    public class RedisCacheSevice : IRedisCacheService
    {
        private readonly IDistributedCache? _cache;
        public RedisCacheSevice(IDistributedCache cache)
        {
            _cache = cache;
        }
        public T? GetData<T>(string key)
        {
            var data = _cache.GetString(key);
            if (data is null)
                return default(T);

            return JsonSerializer.Deserialize<T>(data);
        }

        public void SetData<T>(string key, T value)
        {
            var options = new DistributedCacheEntryOptions()
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)

            };
            _cache.SetString(key, JsonSerializer.Serialize(value), options);
        }
    }
}
