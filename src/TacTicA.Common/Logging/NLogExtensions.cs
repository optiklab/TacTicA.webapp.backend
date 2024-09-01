using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NLog;
using NLog.Web;
using NLog.Extensions.Logging;
using NLog.LayoutRenderers;
using NLog.Targets;

namespace TacTicA.Common.Logging;

public static class NLogExtensions
{
    public static void AddNLogLogging(this IServiceCollection services)
    {
        LogManager.Setup().SetupExtensions(ext =>
        {
            ext.RegisterTarget<DiagnosticListenerTarget>();
            ext.RegisterLayoutRenderer<ActivityTraceLayoutRenderer>();
        });

        services
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder
                    .ClearProviders()
                    .Configure(options =>
                        options.ActivityTrackingOptions =
                            ActivityTrackingOptions.SpanId
                            | ActivityTrackingOptions.TraceId
                            | ActivityTrackingOptions.ParentId
                            | ActivityTrackingOptions.Baggage
                            | ActivityTrackingOptions.Tags);

                loggingBuilder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();
                loggingBuilder.AddNLogWeb();
            });
    }

    public static IHostApplicationBuilder AddNLogLogging(this IHostApplicationBuilder builder)
    {
        LogManager.Setup().SetupExtensions(ext =>
        {
            ext.RegisterTarget<DiagnosticListenerTarget>();
            ext.RegisterLayoutRenderer<ActivityTraceLayoutRenderer>();
        });

        builder.Services
            .AddLogging(loggingBuilder =>
            {
                loggingBuilder
                    .ClearProviders()
                    .Configure(options =>
                        options.ActivityTrackingOptions =
                            ActivityTrackingOptions.SpanId
                            | ActivityTrackingOptions.TraceId
                            | ActivityTrackingOptions.ParentId
                            | ActivityTrackingOptions.Baggage
                            | ActivityTrackingOptions.Tags);

                loggingBuilder.Services.AddSingleton<ILoggerProvider, NLogLoggerProvider>();
                loggingBuilder.AddNLogWeb();
            });

        return builder;
    }
}
