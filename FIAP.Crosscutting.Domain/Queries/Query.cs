using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FIAP.Crosscutting.Domain.Queries.Events;
using FluentValidation.Results;

namespace FIAP.Crosscutting.Domain.Queries
{
    public abstract class Query<TResponse> : QueryMessage<TResponse>
    {
        public Guid Id { get; set; }
        public DateTime CreatedAt { get; private set; }
        public ValidationResult ValidationResult { get; set; } = new();

        protected Query()
        {
            CreatedAt = DateTime.Now.ToBrazilianTimezone();
        }

        public abstract bool IsValid();
    }
}
