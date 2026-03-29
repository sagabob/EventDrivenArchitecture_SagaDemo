namespace Newsletter.Api.Features.Newsletters.Emails;

public interface IEmailService
{
    Task SendWelcomeEmailAsync(string email);

    Task SendFollowUpEmailAsync(string email);

    Task SendTestEmailAsync(string email);
}
