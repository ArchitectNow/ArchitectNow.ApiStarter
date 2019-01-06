namespace ArchitectNow.ApiStarter.Api.Models.ViewModels
{
    public class LoginVm
    {
        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }
        
        /// <summary>
        /// Password (Min 5 characters)
        /// </summary>
        public string Password { get; set; }
    }
}