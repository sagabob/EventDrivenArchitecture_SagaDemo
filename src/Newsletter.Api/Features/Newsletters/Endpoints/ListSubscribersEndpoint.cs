using FastEndpoints;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class ListSubscribersEndpoint(NewsletterDbContext dbContext) : Endpoint<EmptyRequest, List<SubscriberResponse>>
{
    public override void Configure()
    {
        Get("/api/newsletters/subscribers");
        AllowAnonymous();
    }

    public override async Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var entities = await dbContext.Subscribers
            .AsNoTracking()
            .OrderByDescending(s => s.SubscribedOnUtc)
            .Take(100)
            .ToListAsync(ct);

        var rows = entities
            .Select(s => new SubscriberResponse(
                s.Id,
                s.Email,
                s.SubscribedOnUtc,
                s.OnboardingStatus.ToString(),
                s.OnboardingCompletedAtUtc,
                s.OnboardingFaultReason))
            .ToList();

        await Send.OkAsync(rows, ct);
    }
}

public record SubscriberResponse(
    Guid Id,
    string Email,
    DateTime SubscribedOnUtc,
    string OnboardingStatus,
    DateTime? OnboardingCompletedAtUtc,
    string? OnboardingFaultReason);
