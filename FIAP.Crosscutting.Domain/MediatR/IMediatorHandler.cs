using FIAP.Crosscutting.Domain.Commands;
using FIAP.Crosscutting.Domain.Events;
using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.Queries;
using MediatR;

namespace FIAP.Crosscutting.Domain.MediatR
{
    public interface IMediatorHandler
    {
        Task SendCommand<T>(T command) where T : Command;
        Task<TResponse> SendCommandResponse<TResponse>(CommandResponse<TResponse> command) where TResponse : class;
        Task<TResponse> SendQuery<TResponse>(Query<TResponse> query) where TResponse : class;
        Task PublishEvent<T>(T @event) where T : Event;
        bool HasNotification();
        INotificationHandler<DomainNotification> GetNotificationHandler();
    }
}
