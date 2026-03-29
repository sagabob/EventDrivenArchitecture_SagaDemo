using MassTransit;
using Newsletter.Api.Features.Newsletters.Emails;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class SendFollowUpEmailHandler(IEmailService emailService) : IConsumer<SendFollowUpEmail>
{
    public async Task Consume(ConsumeContext<SendFollowUpEmail> context)
    {
        await emailService.SendFollowUpEmailAsync(context.Message.Email);

        await context.Publish(new FollowUpEmailSent
        {
            SubscriberId = context.Message.SubscriberId,
            Email = context.Message.Email
        });
    }
}