namespace Newsletter.Api.Features.Newsletters.Configuration;

public class NewsletterOnboardingOptions
{
    public const string SectionPath = "Newsletter:Onboarding";

    /// <summary>
    ///     Pause between saga steps (before publishing the next command/event) so demos can see status change in the UI.
    /// </summary>
    public int StepDelayMilliseconds { get; set; } = 2500;
}