using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace TacTicA.Common.Logging;

public static class LoggingExtensions
{
    public static void AddRawHttpRequestResponseLogging(this IServiceCollection services)
    {
        services.AddHttpLogging(o =>
        {
            o.LoggingFields = HttpLoggingFields.All;
            o.RequestBodyLogLimit = 4096;
            o.ResponseBodyLogLimit = 4096;
        });
    }

    public static IHostApplicationBuilder AddRawHttpRequestResponseLogging(this IHostApplicationBuilder builder)
    {
        builder.Services.AddHttpLogging(o =>
        {
            o.LoggingFields = HttpLoggingFields.All;
            o.RequestBodyLogLimit = 4096;
            o.ResponseBodyLogLimit = 4096;
        });

        return builder;
    }

    public static IApplicationBuilder UseRawHttpRequestResponseLogging(this IApplicationBuilder applicationBuilder)
    {
        var configuration = applicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();

        return applicationBuilder.UseHttpLogging();
    }
}
