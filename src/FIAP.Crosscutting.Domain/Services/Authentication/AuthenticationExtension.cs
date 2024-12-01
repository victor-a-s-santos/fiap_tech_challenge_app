using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace FIAP.Crosscutting.Domain.Services.Authentication
{
    public static class AuthenticationExtension
    {
        public static AuthenticationBuilder AddTechChallengeAuthentication(this IServiceCollection services)
        {
            return services.AddAuthentication(sharedOptions =>
            {
                sharedOptions.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                sharedOptions.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddTechChallengeAuthentication();
        }

        public static AuthenticationBuilder AddTechChallengeAuthentication(this AuthenticationBuilder builder)
            => builder.AddTechChallengeAuthentication(_ => { });

        public static AuthenticationBuilder AddTechChallengeAuthentication(
            this AuthenticationBuilder builder,
            Action<TokenConfig> configureOptions)
        {
            builder.Services.Configure(configureOptions);
            builder.Services.AddSingleton<IConfigureOptions<JwtBearerOptions>, ConfigureJwtOptions>();
            builder.AddJwtBearer();

            return builder;
        }

        private class ConfigureJwtOptions : IConfigureNamedOptions<JwtBearerOptions>
        {
            private readonly TokenConfig _tokenConfig;

            public ConfigureJwtOptions(TokenConfig tokenConfig)
            {
                _tokenConfig = tokenConfig;

                if (string.IsNullOrEmpty(_tokenConfig.HmacSecretKey))
                {
                    throw new Exception("HmacSecretKey is not definied. Perhaps TokenCredentials is not presented at appsettings.json.");
                }
            }

            public void Configure(string name, JwtBearerOptions options)
            {
                var paramsValidation = options.TokenValidationParameters;

                paramsValidation.ValidateIssuerSigningKey = true;
                paramsValidation.ValidateLifetime = true;
                paramsValidation.ClockSkew = TimeSpan.Zero;
                paramsValidation.IssuerSigningKey = new SymmetricSecurityKey
                (
                    Encoding.UTF8.GetBytes(_tokenConfig.HmacSecretKey)
                );
                paramsValidation.ValidAudience = _tokenConfig.Audience;
                paramsValidation.ValidIssuer = _tokenConfig.Issuer;
            }

            public void Configure(JwtBearerOptions options)
            {
                Configure(Options.DefaultName, options);
            }
        }
    }
}
