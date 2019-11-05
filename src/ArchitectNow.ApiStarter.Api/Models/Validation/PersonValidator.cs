using ArchitectNow.ApiStarter.Api.Models.ViewModels;
using FluentValidation;

namespace ArchitectNow.ApiStarter.Api.Models.Validation
{
    internal class PersonValidator : AbstractValidator<UserVm>
    {
        public PersonValidator()
        {
            RuleFor(vm => vm.Email).NotEmpty().EmailAddress();
            RuleFor(vm => vm.NameFirst).MinimumLength(3);
            RuleFor(vm => vm.NameLast).MinimumLength(3);

            //test change
        }
    }
}