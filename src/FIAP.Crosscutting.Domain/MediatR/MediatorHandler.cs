using FIAP.Crosscutting.Domain.Commands;
using FIAP.Crosscutting.Domain.Events;
using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.Queries;
using MediatR;

namespace FIAP.Crosscutting.Domain.MediatR
{
    public sealed class MediatorHandler : IMediatorHandler
    {
        private readonly IMediator _mediator;
        private readonly DomainNotificationHandler _domainNotification;

        public MediatorHandler(
            IMediator mediator,
            INotificationHandler<DomainNotification> domainNotification)
        {
            _mediator = mediator ?? throw new System.ArgumentNullException(nameof(mediator));
            _domainNotification = (DomainNotificationHandler)domainNotification ??
                throw new System.ArgumentNullException(nameof(domainNotification));
        }

        public Task SendCommand<T>(T command) where T : Command
        {
            return _mediator.Send(command);
        }

        public Task<TResponse> SendCommandResponse<TResponse>(CommandResponse<TResponse> command) where TResponse : class
        {
            return _mediator.Send(command);
        }

        public Task<TResponse> SendQuery<TResponse>(Query<TResponse> query) where TResponse : class
        {
            return _mediator.Send(query);
        }

        public Task PublishEvent<T>(T @event) where T : Event
        {
            return _mediator.Publish(@event);
        }

        public bool HasNotification()
        {
            return _domainNotification.HasNotifications();
        }

        public INotificationHandler<DomainNotification> GetNotificationHandler()
        {
            return _domainNotification;
        }
    }
}
