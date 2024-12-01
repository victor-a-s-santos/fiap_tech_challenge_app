using NSwag;

namespace FIAP.TechChallenge.Api.Configurations
{
    public static class SwaggerSetup
    {
        public static void AddSwaggerSetup(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddSwaggerDocument(config =>
            {
                config.UseControllerSummaryAsTagDescription = true;

                config.AddSecurity("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme.Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = OpenApiSecurityApiKeyLocation.Header,
                    Type = OpenApiSecuritySchemeType.ApiKey
                });

                config.PostProcess = document =>
                {
                    document.Info.Version = "v1";
                    document.Info.Title = "Documentação FIAP Tech Challenge API";
                    document.Info.Description = "API de serviços voltados a plataforma FIAP Tech Challenge";
                };
            });
        }

        public static void UseSwaggerSetup(this IApplicationBuilder app)
        {
            if (app == null)
                throw new ArgumentNullException(nameof(app));

            app.UseOpenApi();

            app.UseSwaggerUi3(opt =>
            {
                opt.Path = "/docs";
            });
        }
    }
}
