namespace Newsletter.Api.Features.Newsletters.Messages;

public record SubscriberCreated
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = string.Empty;
}

public record WelcomeEmailSent
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = string.Empty;
}

public record FollowUpEmailSent
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = string.Empty;
}

public record OnboardingCompleted
{
    public Guid SubscriberId { get; init; }

    public string Email { get; init; } = string.Empty;
}