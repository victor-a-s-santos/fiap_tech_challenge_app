using FIAP.Crosscutting.Domain.Commands.Events;

namespace FIAP.Crosscutting.Domain.Events
{
    public interface IHandler<in T> where T : CommandMessage
    {
        void Handle(T message);
    }
}
