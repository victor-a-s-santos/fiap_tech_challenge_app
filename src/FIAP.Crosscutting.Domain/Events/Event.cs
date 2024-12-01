using FIAP.Crosscutting.Domain.Commands.Events;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using MediatR;

namespace FIAP.Crosscutting.Domain.Events
{
    public abstract class Event : CommandMessage, INotification
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; set; }

        public Event()
        {
            CreatedAt = DateTime.Now.ToBrazilianTimezone();
        }
    }
}
