namespace Core.Infrastructure.Redis
{
    public interface IRedisCacheService
    {
        Task<T?> GetDataAsync<T>(string key);
        Task SetDataAsync<T>(string key, T value, TimeSpan? expirationTime = null);
        Task RemoveDataAsync(string key);
        Task RemoveByPatternAsync(string pattern);
    }
}
