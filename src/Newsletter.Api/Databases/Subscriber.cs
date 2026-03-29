namespace Newsletter.Api.Databases;

public class Subscriber
{
    public Guid Id { get; set; }

    public string Email { get; set; } = string.Empty;

    public DateTime SubscribedOnUtc { get; set; }

    public SubscriberOnboardingStatus OnboardingStatus { get; set; } = SubscriberOnboardingStatus.Pending;

    public DateTime? OnboardingCompletedAtUtc { get; set; }

    public string? OnboardingFaultReason { get; set; }
}