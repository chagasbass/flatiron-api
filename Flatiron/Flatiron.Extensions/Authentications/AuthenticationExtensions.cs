namespace Flatiron.Extensions.Authentications;

public static class AuthenticationExtensions
{
    public static IServiceCollection AddApiAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.Audience = "https://identitytoolkit.googleapis.com/google.identity.identitytoolkit.v1.IdentityToolkit";
                options.Authority = "https://securetoken.google.com/liveretail-5366e";
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = "https://securetoken.google.com/liveretail-5366e",
                    ValidateAudience = true,
                    ValidAudience = "liveretail-5366e",
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

        return services;
    }
}
