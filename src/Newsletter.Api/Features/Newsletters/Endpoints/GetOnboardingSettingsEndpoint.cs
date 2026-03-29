using FastEndpoints;
using Microsoft.Extensions.Options;
using Newsletter.Api.Features.Newsletters.Configuration;

namespace Newsletter.Api.Features.Newsletters.Endpoints;

public class GetOnboardingSettingsEndpoint(IOptions<NewsletterOnboardingOptions> options)
    : Endpoint<EmptyRequest, OnboardingSettingsResponse>
{
    public override void Configure()
    {
        Get("/api/newsletters/onboarding-settings");
        AllowAnonymous();
    }

    public override Task HandleAsync(EmptyRequest req, CancellationToken ct)
    {
        var v = options.Value;
        return Send.OkAsync(new OnboardingSettingsResponse(v.StepDelayMilliseconds), ct);
    }
}

public record OnboardingSettingsResponse(int StepDelayMilliseconds);