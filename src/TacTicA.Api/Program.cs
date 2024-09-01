using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

namespace TacTicA.ApiGateway
{
    public class Program
    {
        public static void Main(string[] args)
        {
            /* TODO
            var builder =  WebApplication.CreateBuilder(args);

            builder
                .AddNLogLogging()
                .AddRawHttpRequestResponseLogging();
            */

            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
