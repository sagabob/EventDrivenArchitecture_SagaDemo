using brevo_csharp.Api;
using brevo_csharp.Model;
using Microsoft.Extensions.Options;
using Task = System.Threading.Tasks.Task;

namespace Newsletter.Api.Features.Newsletters.Emails;

public class BrevoEmailService(ITransactionalEmailsApi emailsApi, IOptions<BrevoOptions> options) : IEmailService
{
    public async Task SendWelcomeEmailAsync(string email)
    {
        await SendAsync(email, "Subscriber", "Welcome to our Newsletter!",
            "<h1>Welcome!</h1><p>We are thrilled to have you onboard.</p>");
    }

    public async Task SendFollowUpEmailAsync(string email)
    {
        await SendAsync(email, "Subscriber", "How are you liking the newsletter?",
            "<p>Just checking in to see if you are enjoying our content!</p>");
    }

    public async Task SendTestEmailAsync(string email)
    {
        await SendAsync(email, "Subscriber", "Testing sending newsletter?",
            "<p>A quick check to see whether the sending newsletter service is working!</p>");
    }

    private async Task SendAsync(string toEmail, string toName, string subject, string htmlBody)
    {
        var fromEmail = options.Value.SenderEmail;
        var fromName = options.Value.SenderName;
        var email = new SendSmtpEmail(
            new SendSmtpEmailSender(fromName, fromEmail),
            [new SendSmtpEmailTo(toEmail, toName)],
            subject: subject,
            htmlContent: htmlBody
        );

        await emailsApi.SendTransacEmailAsync(email);
    }
}