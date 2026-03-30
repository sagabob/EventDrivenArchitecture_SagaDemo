using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Emails;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class SendFollowUpEmailHandler(NewsletterDbContext dbContext, IEmailService emailService)
    : IConsumer<SendFollowUpEmail>
{
    public async Task Consume(ConsumeContext<SendFollowUpEmail> context)
    {
        var subscriber = await dbContext.Subscribers
            .FirstOrDefaultAsync(s => s.Id == context.Message.SubscriberId, context.CancellationToken);

        if (subscriber is null)
            throw new InvalidOperationException($"Subscriber {context.Message.SubscriberId} was not found.");

        subscriber.OnboardingStatus = SubscriberOnboardingStatus.FollowingUp;
        subscriber.OnboardingFaultReason = null;
        await dbContext.SaveChangesAsync(context.CancellationToken);

        await emailService.SendFollowUpEmailAsync(context.Message.Email);

        await context.Publish(new FollowUpEmailSent
        {
            SubscriberId = context.Message.SubscriberId,
            Email = context.Message.Email
        });

        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}