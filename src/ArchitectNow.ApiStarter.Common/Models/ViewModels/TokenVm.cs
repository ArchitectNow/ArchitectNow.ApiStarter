using System;

namespace ArchitectNow.ApiStarter.Common.Models.ViewModels
{
    public class TokenVm
    {
        public string Token { get; set; }
        public DateTimeOffset ExpiresAt { get; set; }
        public string Email { get; set; }
    }
}