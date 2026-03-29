using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Emails;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class SendWelcomeEmailHandler(NewsletterDbContext dbContext, IEmailService emailService)
    : IConsumer<SendWelcomeEmail>
{
    public async Task Consume(ConsumeContext<SendWelcomeEmail> context)
    {
        var subscriber = await dbContext.Subscribers
            .FirstOrDefaultAsync(s => s.Id == context.Message.SubscriberId, context.CancellationToken);

        if (subscriber is not null)
        {
            subscriber.OnboardingStatus = SubscriberOnboardingStatus.Welcoming;
            subscriber.OnboardingFaultReason = null;
            await dbContext.SaveChangesAsync(context.CancellationToken);
        }

        await emailService.SendWelcomeEmailAsync(context.Message.Email);

        await context.Publish(new WelcomeEmailSent
        {
            SubscriberId = context.Message.SubscriberId,
            Email = context.Message.Email
        });

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}