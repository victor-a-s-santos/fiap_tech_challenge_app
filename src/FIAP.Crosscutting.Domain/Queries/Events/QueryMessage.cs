using FIAP.Crosscutting.Domain.Events;
using MediatR;

namespace FIAP.Crosscutting.Domain.Queries.Events
{
    public class QueryMessage<TResponse> : IRequest<TResponse>, IRequestBase
    {
        public string MessageType { get; set; }

        protected QueryMessage()
        {
            MessageType = GetType().Name;
        }
    }
}
