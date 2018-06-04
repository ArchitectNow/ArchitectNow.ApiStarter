using ArchitectNow.ApiStarter.Common.Models.ViewModels;
using ArchitectNow.ApiStarter.Common.Validators;
using FluentValidation;

namespace ArchitectNow.ApiStarter.Common.Models.Validation
{
    internal class RegistrationValidator : AbstractValidator<RegistrationVm>
    {
        public RegistrationValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().EmailAddress();
            RuleFor(vm => vm.NameFirst).MinimumLength(3);
            RuleFor(vm => vm.NameLast).MinimumLength(3);
            RuleFor(vm => vm.Password).Password().WithMessage("Password must be 6 numbers only");

        }
    }
}