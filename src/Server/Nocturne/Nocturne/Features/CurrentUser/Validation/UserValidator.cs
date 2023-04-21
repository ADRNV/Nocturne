using FluentValidation;

namespace Nocturne.Features.CurrentUser.Validation
{
    public class UserValidator : AbstractValidator<CoreUser>
    {
        public UserValidator()
        {
            RuleFor(u => u.Login).NotEmpty();
            RuleFor(u => u.UserName).NotEmpty();
        }
    }
}
