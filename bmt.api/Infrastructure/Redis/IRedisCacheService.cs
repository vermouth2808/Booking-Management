﻿namespace Core.Infrastructure.Redis
{
    public interface IRedisCacheService
    {
        T? GetData<T>(string key); 
        void SetData<T> (string key, T value);
    }
}
