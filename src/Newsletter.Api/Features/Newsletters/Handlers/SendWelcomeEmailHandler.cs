using MassTransit;
using Newsletter.Api.Features.Newsletters.Emails;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class SendWelcomeEmailHandler(IEmailService emailService) : IConsumer<SendWelcomeEmail>
{
    public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
    {
        await emailService.SendWelcomeEmailAsync(context.Message.Email);

        await context.Publish(new WelcomeEmailSent
        {
            SubscriberId = context.Message.SubscriberId,
            Email = context.Message.Email
        });
    }
}