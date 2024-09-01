using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;


namespace TacTicA.Common.OpenApi;

public static class SwaggerBootstrap
{
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.AddSecurityDefinition(name: "Bearer", securityScheme: new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Description =
                    "Application uses JWT based bearer authentication/authorization." + Environment.NewLine +
                    "Refer https://swagger.io/docs/specification/authentication/bearer-authentication/ for more details." + Environment.NewLine +
                    "Authorization HTTP header has the following format:" + Environment.NewLine +
                    "Bearer <token>" + Environment.NewLine +
                    "The token value can be obtained via Identity API",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer",
                BearerFormat = "JWT",
            });

            var securityDefinition = new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            };

            var securityRequirement = new OpenApiSecurityRequirement
             {
                    { securityDefinition, Array.Empty<string>() }
             };

            options.AddSecurityRequirement(securityRequirement);
        });
    }
}