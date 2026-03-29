using MassTransit;
using Newsletter.Api.Features.Newsletters.Messages;

namespace Newsletter.Api.Features.Newsletters.Sagas;

public class NewsletterOnboardingSaga : MassTransitStateMachine<NewsletterOnboardingSagaData>
{
    public NewsletterOnboardingSaga()
    {
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
                .Publish(context => new SendWelcomeEmail(context.Saga.SubscriberId, context.Saga.Email))
                .TransitionTo(Welcoming));

        During(Welcoming,
            When(WelcomeEmailSent)
                .Then(context => context.Saga.WelcomeEmailSent = true)
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
    public State Faulted { get; } = null!;

    public Event<SubscriberCreated> SubscriberCreated { get; } = null!;
    public Event<WelcomeEmailSent> WelcomeEmailSent { get; } = null!;
    public Event<FollowUpEmailSent> FollowUpEmailSent { get; } = null!;
    public Event<Fault<SendWelcomeEmail>> SendWelcomeEmailFaulted { get; } = null!;
    public Event<Fault<SendFollowUpEmail>> SendFollowUpEmailFaulted { get; } = null!;
}