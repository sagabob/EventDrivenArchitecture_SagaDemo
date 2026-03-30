using brevo_csharp.Api;
using brevo_csharp.Client;
using FastEndpoints;
using FastEndpoints.Swagger;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newsletter.Api.Databases;
using Newsletter.Api.Extensions;
using Newsletter.Api.Features.Newsletters.Configuration;
using Newsletter.Api.Features.Newsletters.Emails;
using Newsletter.Api.Features.Newsletters.Extensions;
using Newsletter.Api.Features.Newsletters.Services;

var builder = WebApplication.CreateBuilder();

// Register FastEndpoints and configure its Swagger document once
builder.Services.AddFastEndpoints()
    .SwaggerDocument(o =>
    {
        o.DocumentSettings = s =>
        {
            s.Title = "Newsletter API";
            s.Version = "v1";
        };
    });

builder.Services.AddProblemDetails();

builder.Services.AddDbContext<NewsletterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<ISubscribeNewsletterService, SubscribeToNewsletterService>();

builder.Services.AddNewsletterMassTransit(builder.Configuration);

builder.Services.Configure<NewsletterOnboardingOptions>(
    builder.Configuration.GetSection(NewsletterOnboardingOptions.SectionPath));

builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection(BrevoOptions.SectionName));

builder.Services.AddTransient<ITransactionalEmailsApi>(sp =>
{
    var options = sp.GetRequiredService<IOptions<BrevoOptions>>().Value;

    var config = new Configuration
    {
        ApiKey = { ["api-key"] = options.ApiKey }
    };

    return new TransactionalEmailsApi(config);
});

builder.Services.AddTransient<IEmailService, BrevoEmailService>();


var app = builder.Build();

app.UseGlobalExceptionHandling();

app.UseDefaultFiles();
app.UseStaticFiles();

// Enable endpoints and FastEndpoints' Swagger middleware
app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();