using Microsoft.Extensions.Caching.Memory;

namespace ArchitectNow.ApiStarter.Common.Services
{
    public class MemoryCacheService : ICacheService
    {
        private readonly IMemoryCache _memoryCache;
        
        public MemoryCacheService(IMemoryCache memoryCache)
        {
            _memoryCache = memoryCache;
        }
        
        public void Add(string key, object value)
        {
            _memoryCache.Set(key, value);
        }

        public T Get<T>(string key)
        {
            return _memoryCache.Get<T>(key);
        }

        public object Get(string key)
        {
            return _memoryCache.Get(key);
        }

        public void Remove(string key)
        {
            _memoryCache.Remove(key);
        }

        public void ClearCache()
        {
            //TODO:  No easy implementation in MemoryCache
        }
    }
}