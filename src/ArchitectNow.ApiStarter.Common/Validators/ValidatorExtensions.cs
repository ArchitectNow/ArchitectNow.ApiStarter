using FluentValidation;

namespace ArchitectNow.ApiStarter.Common.Validators
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> Password<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder.SetValidator(new PasswordValidator());
        }
    }
}