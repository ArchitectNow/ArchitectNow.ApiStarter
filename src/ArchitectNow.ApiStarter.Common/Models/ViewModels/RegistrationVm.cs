using System.ComponentModel.DataAnnotations;

namespace ArchitectNow.ApiStarter.Common.Models.ViewModels
{
    public class RegistrationVm
    {
        public RegistrationVm()
        {
            
        }
        
        [Required]
        public string Email { get; set; }
        
        [Required]
        public string Password { get; set; }
        
        [Required]
        public string NameFirst { get; set; }
        
        [Required]
        public string NameLast { get; set; }
    }
}