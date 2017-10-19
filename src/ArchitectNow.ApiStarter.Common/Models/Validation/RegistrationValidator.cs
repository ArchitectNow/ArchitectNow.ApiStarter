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
            RuleFor(vm => vm.NameFirst).Length(3);
            RuleFor(vm => vm.NameLast).Length(3);
            RuleFor(vm => vm.Password).Password().WithMessage("Password must be 4 numbers only");
        }
    }
}