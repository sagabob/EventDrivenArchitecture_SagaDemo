using MassTransit;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Handlers;

public class SendFollowUpEmailFaultedHandler(ILogger<SendFollowUpEmailFaultedHandler> logger)
    : IConsumer<Fault<SendFollowUpEmail>>
{
    public Task Consume(ConsumeContext<Fault<SendFollowUpEmail>> context)
    {
        var originalCommand = context.Message.Message;
        var exceptions = context.Message.Exceptions;

        var errorMessage = exceptions.FirstOrDefault()?.Message ?? "Unknown error";

        // Emulating sending a Slack or PagerDuty notification to the development team
        logger.LogWarning(
            "ALERT [Developer Notification]: Follow-up email failed permanently for {Email}. Expected Manual intervention. Reason: {ErrorMessage}",
            originalCommand.Email, errorMessage);

        return Task.CompletedTask;
    }
}