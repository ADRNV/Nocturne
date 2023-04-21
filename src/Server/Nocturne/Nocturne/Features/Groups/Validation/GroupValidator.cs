using FluentValidation;

namespace Nocturne.Features.Groups.Validation
{
    public class GroupValidator : AbstractValidator<CoreGroup>
    {
        public GroupValidator()
        {
            RuleFor(g => g.Name).NotEmpty();
            RuleFor(g => g.Users.Count()).GreaterThan(1);
        }
    }
}
