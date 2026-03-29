namespace Newsletter.Api.Features.Newsletters.Emails;

public class BrevoOptions
{
    public const string SectionName = "Brevo";
    
    public string ApiKey { get; set; } = string.Empty;
    public string SenderName { get; set; } = string.Empty;
    public string SenderEmail { get; set; } = string.Empty;
}
