using FastEndpoints;
using FluentValidation;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Validators;

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