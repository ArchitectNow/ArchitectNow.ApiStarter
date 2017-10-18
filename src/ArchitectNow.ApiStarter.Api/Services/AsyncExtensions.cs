using System.Threading.Tasks;

namespace ArchitectNow.ApiStarter.Api.Services
{
    public static class AsyncExtensions
    {
        public static Task<T> AsResult<T>(this T result)
        {
            return Task.FromResult(result);
        }
    }
}