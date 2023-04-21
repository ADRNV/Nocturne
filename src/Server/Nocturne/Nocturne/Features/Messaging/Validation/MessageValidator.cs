using FluentValidation;

namespace Nocturne.Features.Messaging.Validation
{
    public class MessageValidator : AbstractValidator<CoreMessage>
    {
        public MessageValidator()
        {
            RuleFor(m => m.From).NotEmpty();
            RuleFor(m => m.Content).NotEmpty();
        }
    }
}
