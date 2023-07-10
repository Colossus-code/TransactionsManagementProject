﻿using Microsoft.Extensions.Caching.Memory;
using TransactionsManagementProject.Contracts.RepositoryContracts;

namespace TransactionsManagementProject.InfrastructureData
{
    public class RepositoryCache : IRepositoryCache
    {
        private readonly IMemoryCache _memoryCache;
        public RepositoryCache(IMemoryCache memoryCache) 
        {
            _memoryCache = memoryCache;

        }

        public T GetCache<T>(string key, T defaultValue)
        {
            var response = _memoryCache.Get(key);

            if (response == null) return defaultValue;

            return (T)response; 
        }

        public List<T> GetCacheList<T>(string key)
        {
            var response = _memoryCache.Get(key);

            return (List<T>)response;
        }

        public void SetCache<T>(string key, T generic)
        {
            _memoryCache.Set(key, generic);
        }

        public void SetCacheList<T>(string key, List<T> generic) 
        {
            _memoryCache.Set(key, generic);
        }
    }
}