using FluentValidation;

namespace Nocturne.Features.CurrentUser.Validation
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.User).SetValidator(new UserValidator());
        }
    }
}
