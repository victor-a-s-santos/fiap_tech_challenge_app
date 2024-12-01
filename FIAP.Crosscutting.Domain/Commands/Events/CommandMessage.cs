using FIAP.Crosscutting.Domain.Events;
using MediatR;
using System.Text.Json.Serialization;

namespace FIAP.Crosscutting.Domain.Commands.Events
{
    public abstract class CommandMessage : IRequest, IRequestBase
    {
        [JsonIgnore]
        public string MessageType { get; protected set; }

        protected CommandMessage()
        {
            MessageType = GetType().Name;
        }
    }
}
