namespace ArchitectNow.ApiStarter.Common.Services
{
    public interface ICacheService
    {
        void Add(string key, object value);
        T Get<T>(string key);
        object Get(string key);
        void Remove(string key);
        void ClearCache();
    }
}