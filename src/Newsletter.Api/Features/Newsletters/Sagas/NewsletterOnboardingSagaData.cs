using MassTransit;

namespace Newsletter.Api.Features.Newsletters.Sagas;

public class NewsletterOnboardingSagaData : SagaStateMachineInstance
{
    public string CurrentState { get; set; } = string.Empty;

    public Guid SubscriberId { get; set; }
    public string Email { get; set; } = string.Empty;
    public bool WelcomeEmailSent { get; set; }
    public bool FollowUpEmailSent { get; set; }
    public bool OnboardingCompleted { get; set; }
    public string? ErrorMessage { get; set; }

    public byte[]? RowVersion { get; set; }

    public Guid CorrelationId { get; set; }
}