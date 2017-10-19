using System.Text.RegularExpressions;
using FluentValidation.Resources;
using FluentValidation.Validators;

namespace ArchitectNow.ApiStarter.Common.Validators
{
    public class PasswordValidator : PropertyValidator, IRegularExpressionValidator
    {
        private readonly Regex _regex;

        public PasswordValidator()
            : base(new LanguageStringSource(nameof(EmailValidator)))
        {
            _regex = new Regex(Expression);
        }

        public string Expression { get; } = "^[0-9]{6}$";

        protected override bool IsValid(PropertyValidatorContext context)
        {
            return context.PropertyValue == null || _regex.IsMatch((string) context.PropertyValue);
        }
    }
}