using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.Helpers.Validators;
using FIAP.Crosscutting.Domain.MediatR;
using Microsoft.AspNetCore.Mvc;

namespace FIAP.Crosscutting.Domain.Controller
{
    public abstract class ApiController : ControllerBase
    {
        private readonly DomainNotificationHandler _notifications;
        protected IMediatorHandler _mediator { get; }

        protected ApiController(IMediatorHandler mediator)
        {
            _notifications = (DomainNotificationHandler)mediator.GetNotificationHandler();
            _mediator = mediator;
        }

        protected IEnumerable<DomainNotification> Notifications => _notifications.GetNotifications();

        protected bool IsValidOperation()
        {
            return (!_notifications.HasNotifications());
        }

        protected new IActionResult Response(object result = null)
        {
            if (IsValidOperation())
                return Ok(result);

            return BadRequest(new BadRequestResponse(_notifications.GetNotifications().Select(n => n.Value)));
        }

        protected IActionResult ModalStateResponse()
        {
            NotifyModelStateErrors();

            return Response();
        }

        protected IActionResult ResponseWithError(string error)
        {
            NotifyError(error);

            return Response();
        }

        protected void NotifyModelStateErrors()
        {
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            foreach (var error in errors)
            {
                var erroMsg = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotifyError(string.Empty, erroMsg);
            }
        }

        protected IEnumerable<string> GetNotificationErrors()
        {
            return _notifications.GetNotifications().Select(t => t.Value);
        }

        protected void NotifyError(string code, string message)
        {
            _mediator.PublishEvent(new DomainNotification(code, message));
        }

        protected void NotifyError(string message) => NotifyError(string.Empty, message);

        protected bool IsNullRequest(object request)
        {
            if (request != null) return false;

            NotifyError("O objeto informado é inválido. Verifique os parâmetros passados e tente novamente.");

            return true;
        }

        protected bool ValidateStringToGuidParams(string guid)
        {
            bool isValid = Validator.IsGuid(guid);

            if (!isValid)
                NotifyError("O parâmetro informado não é válido, por favor informe um valor de padrão UUID.");

            return isValid;
        }

        protected bool IsLocalEnvironment()
        {
            bool isLocalEnvironment = false;

            if (Request.Headers.ContainsKey("Origin"))
            {
                var originValue = Request.Headers["Origin"].FirstOrDefault();
                isLocalEnvironment = originValue.Contains("localhost");
            }

            return isLocalEnvironment;
        }
    }

    public class BadRequestResponse
    {
        public IEnumerable<string> Errors { get; }

        public BadRequestResponse(IEnumerable<string> errors)
        {
            Errors = errors;
        }
    }

    public class UnauthorizedResponse
    {
        public UnauthorizedResponse() { }
    }
}
