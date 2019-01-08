namespace ArchitectNow.ApiStarter.Common.Models.ViewModels
{
    public class RegistrationVm
    {
        /// <summary>
        ///     Email for new user
        /// </summary>
        /// <example>kvgros@architectnow.net</example>
        public string Email { get; set; }

        /// <summary>
        ///     Password for new user
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     First name
        /// </summary>
        public string NameFirst { get; set; }

        /// <summary>
        ///     Last Name
        /// </summary>
        public string NameLast { get; set; }
    }
}