using FIAP.Crosscutting.Domain.Commands.Events;
using FIAP.Crosscutting.Domain.Helpers.Extensions;
using FluentValidation.Results;
using System.Text.Json.Serialization;

namespace FIAP.Crosscutting.Domain.Commands
{
    public abstract class Command : CommandMessage
    {
        public Guid Id { get; set; }

        [JsonIgnore]
        public DateTime Timestamp { get; private set; }

        [JsonIgnore]
        public bool ExecutedSuccessfullyCommand { get; set; } = false;

        [JsonIgnore]
        public Guid ExecutedCommandUserId { get; set; }

        [JsonIgnore]
        public ValidationResult ValidationResult { get; set; } = new();

        protected Command()
        {
            Timestamp = DateTime.Now.ToBrazilianTimezone();
        }

        public abstract bool IsValid();
    }
}
