using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class OnboardingCompletedHandler(NewsletterDbContext dbContext, ILogger<OnboardingCompletedHandler> logger)
    : IConsumer<OnboardingCompleted>
{
    public async Task Consume(ConsumeContext<OnboardingCompleted> context)
    {
        var subscriber = await dbContext.Subscribers
            .FirstOrDefaultAsync(s => s.Id == context.Message.SubscriberId, context.CancellationToken);

        if (subscriber is null)
            throw new InvalidOperationException($"Subscriber {context.Message.SubscriberId} was not found.");


        subscriber.OnboardingStatus = SubscriberOnboardingStatus.Completed;
        subscriber.OnboardingCompletedAtUtc = DateTime.UtcNow;
        subscriber.OnboardingFaultReason = null;
        await dbContext.SaveChangesAsync(context.CancellationToken);

        logger.LogInformation("Onboarding completed for subscriber {SubscriberId}", context.Message.SubscriberId);
    }
}