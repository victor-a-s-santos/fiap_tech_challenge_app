using FIAP.Crosscutting.Domain.Services.Authentication;

namespace FIAP.TechChallenge.Api.Configurations
{
    public static class TokenCredentialSetup
    {
        public static void AddTokenCredentialSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var config = new TokenConfig();
            configuration.Bind("TokenCredentials", config);

            services.AddSingleton(config);
        }
    }
}
