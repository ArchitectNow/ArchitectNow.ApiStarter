using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using FluentValidation;

namespace ArchitectNow.ApiStarter.Api.Models.Validation
{
    public class LoginValidator : AbstractValidator<LoginVm>
    {
        public LoginValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().EmailAddress();
            RuleFor(vm => vm.Password).NotEmpty();
        }
    }
}