using System;

namespace ArchitectNow.ApiStarter.Common.Models.ViewModels
{
    public class UserInformation
    {
        public Guid Id { get; set; }
        public string UserRole { get; set; }
        public string Email { get; set; }
    }
}