using MediatR;

namespace FIAP.Crosscutting.Domain.Events.Notifications
{
    public class DomainNotificationHandler : INotificationHandler<DomainNotification>
    {
        private List<DomainNotification> _notifications;

        public DomainNotificationHandler()
        {
            _notifications = new();
        }

        public Task Handle(DomainNotification message, CancellationToken cancellationToken)
        {
            GetNotifications().Add(message);

            return Task.CompletedTask;
        }

        public virtual List<DomainNotification> GetNotifications()
        {
            return _notifications;
        }

        public virtual bool HasNotifications()
        {
            return GetNotifications().Any();
        }

        public void Dispose()
        {
            _notifications = new();
        }
    }
}
