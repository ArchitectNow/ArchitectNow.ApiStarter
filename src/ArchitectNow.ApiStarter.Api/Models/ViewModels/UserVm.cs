namespace ArchitectNow.ApiStarter.Api.Models.ViewModels
{
    public class UserVm
    {
        /// <summary>
        /// User Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// First Name
        /// </summary>
        public string NameFirst { get; set; }

        /// <summary>
        /// Last Name
        /// </summary>
        public string NameLast { get; set; }

        /// <summary>
        /// Security role for this user
        /// </summary>
        public string UserRole { get; set; }
    }
}