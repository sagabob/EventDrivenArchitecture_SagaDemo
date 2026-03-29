using MassTransit;
using Newsletter.Api.Databases;
using Newsletter.Api.Features.Newsletters.Sagas;

namespace Newsletter.Api.Features.Newsletters.Extensions;

public static class ConfigurationExtensions
{
    public static IServiceCollection AddNewsletterMassTransit(this IServiceCollection services,
        IConfiguration configuration)
    {
        services.AddMassTransit(busConfigurator =>
        {
            busConfigurator.SetKebabCaseEndpointNameFormatter();

            busConfigurator.AddDelayedMessageScheduler();

            busConfigurator.AddConsumers(typeof(ConfigurationExtensions).Assembly);

            busConfigurator.AddSagaStateMachine<NewsletterOnboardingSaga, NewsletterOnboardingSagaData>()
                .EntityFrameworkRepository(r =>
                {
                    r.ConcurrencyMode = ConcurrencyMode.Pessimistic;
                    r.ExistingDbContext<NewsletterDbContext>();
                    r.UsePostgres();
                });

            busConfigurator.AddEntityFrameworkOutbox<NewsletterDbContext>(o =>
            {
                o.UsePostgres();
                o.UseBusOutbox();
            });

            busConfigurator.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(new Uri(configuration.GetConnectionString("RabbitMQ")!));

                cfg.UseDelayedMessageScheduler();

                cfg.UseCircuitBreaker(cb =>
                {
                    cb.TrackingPeriod = TimeSpan.FromMinutes(1);
                    cb.TripThreshold = 15;
                    cb.ActiveThreshold = 10;
                    cb.ResetInterval = TimeSpan.FromMinutes(5);
                });

                cfg.UseDelayedRedelivery(r => r.Intervals(
                    TimeSpan.FromMinutes(5),
                    TimeSpan.FromMinutes(15),
                    TimeSpan.FromMinutes(30)
                ));

                cfg.UseMessageRetry(r => r.Interval(3, TimeSpan.FromSeconds(5)));

                cfg.ConfigureEndpoints(context);
            });
        });

        return services;
    }
}