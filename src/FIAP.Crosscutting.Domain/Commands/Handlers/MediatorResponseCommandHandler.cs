using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.MediatR;
using MediatR;

namespace FIAP.Crosscutting.Domain.Commands.Handlers
{
    public abstract class MediatorResponseCommandHandler<TCommand, TResponse> : IRequestHandler<TCommand, TResponse>
        where TCommand : CommandResponse<TResponse>
        where TResponse : class
    {
        protected IMediatorHandler _mediator { get; }

        protected MediatorResponseCommandHandler(
            IMediatorHandler mediator)
        {
            _mediator = mediator;
        }

        public abstract Task<TResponse> AfterValidation(TCommand request);

        public Task<TResponse> Handle(TCommand request, CancellationToken cancellationToken)
        {
            if (!request.IsValid())
            {
                NotifyValidationErrors(request);

                return Task.FromResult<TResponse>(null);
            }

            return AfterValidation(request);
        }

        protected void NotifyValidationErrors(TCommand message)
        {
            foreach (var error in message.ValidationResult.Errors)
            {
                _mediator.PublishEvent(new DomainNotification(message.MessageType, error.ErrorMessage));
            }
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.PublishEvent(new DomainNotification(code, message));
        }

        protected void NotifyError(string message) => NotifyError(string.Empty, message);

        protected void NotifyError(IEnumerable<string> messages) { foreach (var message in messages) NotifyError(message); }

        protected bool HasNotification() => _mediator.HasNotification();

        protected IEnumerable<string> Errors => ((DomainNotificationHandler)_mediator.GetNotificationHandler())
            .GetNotifications()
            .Select(t => t.Value);
    }
}
