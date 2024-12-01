using FIAP.Crosscutting.Domain.Events.Notifications;
using FIAP.Crosscutting.Domain.Interfaces.Services;
using FIAP.Crosscutting.Domain.MediatR;
using FIAP.Crosscutting.Domain.Services.Cryptography;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace FIAP.TechChallenge.IoC
{
    public static class NativeInjectorBootStrapper
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            #region Crosscutting injections

            services.AddScoped<IMediatorHandler, MediatorHandler>();
            services.AddScoped<INotificationHandler<DomainNotification>, DomainNotificationHandler>();
            services.AddScoped<ICryptographyService, CryptographyService>();

            #endregion

            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.Scan(s => s
               .FromApplicationDependencies(a => a.FullName.StartsWith("FIAP.TechChallenge"))
               .AddClasses().AsMatchingInterface((service, filter) =>
                   filter.Where(i => i.Name.Equals($"I{service.Name}", StringComparison.OrdinalIgnoreCase))).WithScopedLifetime()
               .AddClasses(x => x.AssignableTo(typeof(IMediator))).AsImplementedInterfaces().WithScopedLifetime()
               .AddClasses(x => x.AssignableTo(typeof(IRequestHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
               .AddClasses(x => x.AssignableTo(typeof(IRequestHandler<,>))).AsImplementedInterfaces().WithScopedLifetime()
               .AddClasses(x => x.AssignableTo(typeof(INotificationHandler<>))).AsImplementedInterfaces().WithScopedLifetime()
            );

            //services.AddTransient<IJob, Job>();
            services.AddTransient(s => s.GetService<IHttpContextAccessor>().HttpContext.User);

            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(EventStoreBehavior<,>));
        }
    }
}
