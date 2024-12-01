using FIAP.Crosscutting.Domain.Services.Settings;

namespace FIAP.TechChallenge.Api.Configurations
{
    public static class SettingSetup
    {
        public static void AddSettingSetup(this IServiceCollection services, IConfiguration configuration)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            var config = new SettingConfig();
            configuration.Bind("Settings", config);

            services.AddSingleton(config);
        }
    }
}
