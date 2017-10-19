namespace ArchitectNow.ApiStarter.Api.Client
{
    public interface IBaseClient
    {
        string BaseUrl { get; set; }
        string Token { get; set; }
    }
}