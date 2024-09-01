using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using TacTicA.ApiGateway.EventHandlers;
using TacTicA.ApiGateway.Middleware;
using TacTicA.ApiGateway.Repositories;
using TacTicA.Common.Auth;
using TacTicA.Common.EventModel.NotificationEvents;
using TacTicA.Common.Mongo;
using TacTicA.Common.RabbitMq;
using TacTicA.Common.Logging;
using TacTicA.Common.OpenApi;
using TacTicA.Common.EventModel.CommandEvents;
using TacTicA.Common.EventModel;
using TacTicA.Common.QueueServices;

namespace TacTicA.ApiGateway
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "DomainPolicy";

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        // Examples:
                        // builder.WithOrigins("https://atlassian.com",
                        //     "https://www.atlassian.com",
                        //     "https://*.atlassian.com",
                        //     "https://atlassian.net",
                        //     "https://localhost:5001",
                        //     "https://www.atlassian.net",
                        //     "https://*.atlassian.net",
                        //     "https://tacticaddon.herokuapp.com",
                        //     "https://2f4956d8846b.ngrok.io",
                        //     "https://tactica.xyz",
                        //     "https://tacticapi-87817175.us-east-1.elb.amazonaws.com")
                        //         .AllowAnyHeader()
                        //         .AllowAnyMethod();
                    });
            });
            //services.AddMemoryCache();

            services.AddControllers();
            services.AddLogging();

            services.AddNLogLogging();
            services.AddRawHttpRequestResponseLogging();

            // Link handlers interfaces with handlers.
            services.AddTransient<IEventHandler<UserCreatedNotification>, UserCreatedHandler>();
            services.AddTransient<IEventHandler<CreateUserRejectedNotification>, CreateUserRejectedHandler>();
            services.AddTransient<IEventHandler<ItemCreatedNotification>, ItemCreatedHandler>();
            services.AddTransient<IEventHandler<UserAuthenticatedNotification>, UserAuthenticatedHandler>();
            services.AddTransient<IFlattenedItemRepository, FlattenedItemRepository>();

            services.AddJwt(Configuration);

            services.AddRabbitMq(Configuration, (busConfigurator) =>
            {
                busConfigurator.AddConsumer<UserCreatedHandler>();
                busConfigurator.AddConsumer<CreateUserRejectedHandler>();
                busConfigurator.AddConsumer<ItemCreatedHandler>();
                busConfigurator.AddConsumer<UserAuthenticatedHandler>();
            });
            services.AddTransient<IEventBus, EventBus>();

            services.AddMongoDb(Configuration);

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            services.ConfigureSwagger();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();

                app.UseSwagger();
                app.UseSwaggerUI(); // http://localhost:5000/swagger/index.html
            }
            else
            {
                // This is only ASP.NET WebAPI for Singlepage Application. No Error pages needed.
                ////https://docs.microsoft.com/en-us/aspnet/core/fundamentals/error-handling?view=aspnetcore-2.2
                //app.UseExceptionHandler("/Error");

                // I'm not using any of 443/SSL/HTTPS configuration parameters because I'm using reverse-proxy in front of kestrel server
                // to control certificates and SSL connection.
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseMiddleware<ExceptionHandlingMiddleware>();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
