using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class SendFollowUpEmailFaultHandler(NewsletterDbContext dbContext)
    : IConsumer<Fault<SendFollowUpEmail>>
{
    public async Task Consume(ConsumeContext<Fault<SendFollowUpEmail>> context)
    {
        var cmd = context.Message.Message;
        var reason = context.Message.Exceptions?.FirstOrDefault()?.Message;

        var subscriber = await dbContext.Subscribers
            .FirstOrDefaultAsync(s => s.Id == cmd.SubscriberId, context.CancellationToken);

        if (subscriber is null)
            return;

        subscriber.OnboardingStatus = SubscriberOnboardingStatus.Faulted;
        subscriber.OnboardingFaultReason = reason;
        await dbContext.SaveChangesAsync(context.CancellationToken);
    }
}