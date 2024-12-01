using FIAP.Crosscutting.Domain.Services.Cryptography;

namespace FIAP.TechChallenge.Api.Configurations
{
    public static class CryptographySetup
    {
        public static void AddCryptographySetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var config = new CryptographyConfig();
            configuration.Bind("Cryptography", config);

            services.AddSingleton(config);
        }
    }
}
