using MassTransit;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class DeveloperNotificationConsumer(ILogger<DeveloperNotificationConsumer> logger)
    : IConsumer<Fault<SendWelcomeEmail>>
{
    public Task Consume(ConsumeContext<Fault<SendWelcomeEmail>> context)
    {
        var originalCommand = context.Message.Message;
        var exceptions = context.Message.Exceptions;

        var errorMessage = exceptions.FirstOrDefault()?.Message ?? "Unknown error";

        // Emulating sending a Slack or PagerDuty notification to the development team
        logger.LogWarning(
            "ALERT [Developer Notification]: Welcome email failed permanently for {Email}. Expected Manual intervention. Reason: {ErrorMessage}",
            originalCommand.Email, errorMessage);

        return Task.CompletedTask;
    }
}