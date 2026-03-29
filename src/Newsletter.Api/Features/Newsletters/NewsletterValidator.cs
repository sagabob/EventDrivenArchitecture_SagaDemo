using FastEndpoints;
using FluentValidation;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters;

public class NewsletterValidator : Validator<SubscribeToNewsletter>
{
    public NewsletterValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .WithMessage("Invalid email format");
    }
}