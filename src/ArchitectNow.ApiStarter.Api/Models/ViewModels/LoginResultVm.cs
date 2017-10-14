namespace ArchitectNow.ApiStarter.Api.Models.ViewModels
{
    public class LoginResultVm
    {
        public string AuthToken { get; set; }
        public UserVm CurrentUser { get; set; }
    }
}