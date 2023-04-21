using FluentValidation;

namespace Nocturne.Features.Groups.Validation
{
    public class CommandValidator : AbstractValidator<Command>
    {
        public CommandValidator()
        {
            RuleFor(c => c.UserName).NotEmpty();
            RuleFor(c => c.Group).SetValidator(new GroupValidator());
        }
    }
}
