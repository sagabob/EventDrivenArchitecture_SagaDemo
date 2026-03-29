namespace Newsletter.Api.Features.Newsletters.Messages;

public record SubscribeToNewsletter(string Email);

public record SendWelcomeEmail(Guid SubscriberId, string Email);

public record SendFollowUpEmail(Guid SubscriberId, string Email);

