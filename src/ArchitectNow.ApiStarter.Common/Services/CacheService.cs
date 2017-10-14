namespace ArchitectNow.ApiStarter.Common.Services
{
    public class CacheService : ICacheService
    {
        public void Add(string key, object value)
        {
        }

        public T Get<T>(string key)
        {
            return default(T);
        }

        public object Get(string key)
        {
            return null;
        }

        public void Remove(string key)
        {
        }

        public void ClearCache()
        {
        }
    }
}