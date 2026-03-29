using MassTransit;
using Microsoft.Extensions.Options;
using Newsletter.Api.Features.Newsletters.Configuration;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Sagas;

public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
{
    public NewsletterOnboardingSaga(IOptions<NewsletterOnboardingOptions> options)
    {
        var stepDelay = TimeSpan.FromMilliseconds(Math.Max(0, options.Value.StepDelayMilliseconds));

        InstanceState(x => x.CurrentState);

        Event(() => SubscriberCreated, e => e.CorrelateById(m => m.Message.SubscriberId));
        Event(() => WelcomeEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
        Event(() => FollowUpEmailSent, e => e.CorrelateById(m => m.Message.SubscriberId));
        Event(() => SendWelcomeEmailFaulted, e => e.CorrelateById(m => m.Message.Message.SubscriberId));
        Event(() => SendFollowUpEmailFaulted, e => e.CorrelateById(m => m.Message.Message.SubscriberId));

        Initially(
            When(SubscriberCreated)
                .Then(context =>
                {
                    context.Saga.CorrelationId = context.Message.SubscriberId;
                    context.Saga.SubscriberId = context.Message.SubscriberId;
                    context.Saga.Email = context.Message.Email;
                })
                .ThenAsync(async context => await Task.Delay(stepDelay, context.CancellationToken))
                .Publish(context => new SendWelcomeEmail(context.Saga.SubscriberId, context.Saga.Email))
                .TransitionTo(Welcoming));

        During(Welcoming,
            When(WelcomeEmailSent)
                .Then(context => context.Saga.WelcomeEmailSent = true)
                .ThenAsync(async context => await Task.Delay(stepDelay, context.CancellationToken))
                .Publish(context => new SendFollowUpEmail(context.Saga.SubscriberId, context.Saga.Email))
                .TransitionTo(FollowingUp),
            When(SendWelcomeEmailFaulted)
                .Then(context => { context.Saga.ErrorMessage = context.Message.Exceptions?.FirstOrDefault()?.Message; })
                .TransitionTo(Faulted));

        During(FollowingUp,
            When(FollowUpEmailSent)
                .Then(context =>
                {
                    context.Saga.FollowUpEmailSent = true;
                    context.Saga.OnboardingCompleted = true;
                })
                .TransitionTo(Onboarding)
                .ThenAsync(async context => await Task.Delay(stepDelay, context.CancellationToken))
                .Publish(context => new OnboardingCompleted
                {
                    SubscriberId = context.Saga.SubscriberId,
                    Email = context.Saga.Email
                })
                .Finalize(),
            When(SendFollowUpEmailFaulted)
                .Then(context => { context.Saga.ErrorMessage = context.Message.Exceptions?.FirstOrDefault()?.Message; })
                .TransitionTo(Faulted));

        SetCompletedWhenFinalized();
    }

    public State Welcoming { get; } = null!;
    public State FollowingUp { get; } = null!;
    public State Onboarding { get; } = null!;
    public State Faulted { get; } = null!;

    public Event<SubscriberCreated> SubscriberCreated { get; } = null!;
    public Event<WelcomeEmailSent> WelcomeEmailSent { get; } = null!;
    public Event<FollowUpEmailSent> FollowUpEmailSent { get; } = null!;
    public Event<Fault<SendWelcomeEmail>> SendWelcomeEmailFaulted { get; } = null!;
    public Event<Fault<SendFollowUpEmail>> SendFollowUpEmailFaulted { get; } = null!;
}