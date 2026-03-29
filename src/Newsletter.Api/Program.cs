using FastEndpoints;
using FastEndpoints.Swagger;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Emails;
using Newsletter.Api.Features.Newsletters.Handlers;

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

builder.Services.AddDbContext<NewsletterDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddMassTransit(x =>
{
    // Register the consumer
    x.AddConsumer<TestSendNewsletterHandler>();

    // Configure transport
    x.UsingRabbitMq((context, cfg) =>
    {
        cfg.Host(new Uri(builder.Configuration.GetConnectionString("RabbitMQ")!));

        // Auto-create endpoints for registered consumers
        cfg.ConfigureEndpoints(context);
    });
});

builder.Services.Configure<BrevoOptions>(
    builder.Configuration.GetSection(BrevoOptions.SectionName));

builder.Services.AddTransient<brevo_csharp.Api.ITransactionalEmailsApi>(sp =>
{
    var options = sp.GetRequiredService<Microsoft.Extensions.Options.IOptions<BrevoOptions>>().Value;

    var config = new brevo_csharp.Client.Configuration
    {
        ApiKey = { ["api-key"] = options.ApiKey }
    };

    return new brevo_csharp.Api.TransactionalEmailsApi(config);
});

builder.Services.AddTransient<IEmailService, BrevoEmailService>();


var app = builder.Build();

app.UseDefaultFiles();
app.UseStaticFiles();

// Enable endpoints and FastEndpoints' Swagger middleware
app.UseFastEndpoints()
    .UseSwaggerGen();

app.Run();