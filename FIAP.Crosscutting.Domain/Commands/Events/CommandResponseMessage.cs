using FIAP.Crosscutting.Domain.Events;
using MediatR;

namespace FIAP.Crosscutting.Domain.Commands.Events
{
    public class CommandResponseMessage<TResponse> : IRequest<TResponse>, IRequestBase
    {
        public string MessageType { get; protected set; }

        protected CommandResponseMessage()
        {
            MessageType = GetType().Name;
        }
    }
}
