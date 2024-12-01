using FIAP.Crosscutting.Domain.Helpers.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Text.Json;

namespace FIAP.TechChallenge.Domain.Middlewares
{
    public static class ExceptionMiddleware
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    context.Response.ContentType = "application/json";

                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();
                    var serviceProvider = context.Features.Get<IServiceProvidersFeature>();

                    var host = context.Request.Host;
                    var scheme = context.Request.Scheme;
                    var path = context.Request.Path;
                    var query = context.Request.QueryString;

                    // var errorRepository = serviceProvider.RequestServices.GetService<IErrorEventStoreRepository>();

                    if (contextFeature != null)
                    {
                        var errorResult = new ErrorResult
                        {
                            Url = $"{scheme}://{host}{path}{query}",
                            Errors = new[] {
                                contextFeature.Error.Message,
                                contextFeature.Error.StackTrace,
                                contextFeature.Error.InnerException?.ToString()
                            }
                        };

                        var exception = new ErrorDetails(errorResult, contextFeature.Error);
                        //var error = new ErrorEventStore
                        //{
                        //    Url = $"{scheme}://{host}{path}{query}",
                        //    Error = exception.ToString()
                        //};

                        //await errorRepository.AddAsync(error);

                        await context.Response.WriteAsync("Nossos servidores estão indisponíveis no momento. Por favor, tente mais tarde.");
                    }
                });
            });
        }
    }

    public class ErrorDetails : ErrorResult
    {
        public Exception Details { get; set; }

        public ErrorDetails(ErrorResult errorResult, Exception exception)
        {
            Id = errorResult.Id;
            CreatedAt = errorResult.CreatedAt;
            Errors = errorResult.Errors;
            Url = errorResult.Url;

            Details = exception;
        }
    }

    public class ErrorResult
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public IEnumerable<string> Errors { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now.ToBrazilianTimezone();
        public string Url { get; set; }

        public override string ToString()
        {
            return JsonSerializer.Serialize(this);
        }
    }
}
