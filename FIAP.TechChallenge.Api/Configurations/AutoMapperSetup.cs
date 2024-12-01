using FIAP.TechChallenge.Application.Mappings;

namespace FIAP.TechChallenge.Api.Configurations
{
    public static class AutoMapperSetup
    {
        public static void AddAutoMapperSetup(this IServiceCollection services)
        {
            if (services == null)
                throw new ArgumentNullException(nameof(services));

            services.AddAutoMapper(
                typeof(ToViewModelMappingProfile),
                typeof(ToDomainMappingProfile));
        }
    }
}
