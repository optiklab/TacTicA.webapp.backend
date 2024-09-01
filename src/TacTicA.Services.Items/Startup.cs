using TacTicA.Common.Mongo;
using TacTicA.Common.RabbitMq;
using TacTicA.Services.Items.Domain.Repositories;
using TacTicA.Services.Items.Repositories;
using TacTicA.Services.Items.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MongoDB.Bson.Serialization;
using TacTicA.Services.Items.Domain.Models;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson;
using TacTicA.Services.Items.EventHandlers;
using TacTicA.Common.EventModel.CommandEvents;
using TacTicA.Common.EventModel;
using TacTicA.Common.EventModel.NotificationEvents;
using TacTicA.Common.QueueServices;

namespace TacTicA.Services.Items
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
                    });
            });
            //services.AddMemoryCache();

            services.AddControllers();
            services.AddLogging();

            // Link handlers interfaces with handlers.
            services.AddTransient<IEventHandler<CreateItemCommand>, CreateItemHandler>();
            services.AddTransient<IEventHandler<CreateItemRejectedNotification>, CreateItemRejectedHandler>();
            services.AddTransient<ICategoryRepository, CategoryRepository>();
            services.AddTransient<IItemRepository, ItemRepository>();
            services.AddTransient<IItemService, ItemService>();

            // Our custom configuration of RMQ.
            services.AddRabbitMq(Configuration, (busConfigurator) =>
            {
                busConfigurator.AddConsumer<CreateItemHandler>();
                busConfigurator.AddConsumer<CreateItemRejectedHandler>();
            });
            services.AddTransient<IEventBus, EventBus>();

            //public static void AddMongoDbMappers(this IServiceCollection services, IConfiguration configuration)
            //{
            BsonClassMap.RegisterClassMap<Item>(cm =>
            {
                // Perform auto-mapping to include properties without specific mappings
                cm.AutoMap();
                // Serialize string as ObjectId
                cm.MapIdMember(x => x.Id)
                  .SetSerializer(new GuidSerializer(GuidRepresentation.Standard)); //.SetSerializer(new StringSerializer(BsonType.ObjectId));
                // Serialize Guid as binary subtype 4
                cm.MapMember(x => x.UserId).SetSerializer(new GuidSerializer(GuidRepresentation.Standard));
                cm.MapCreator(p => new Item(p.Id, p.CategoryName, p.UserId, p.Name, p.Description, p.CreatedAt));
            });
            //}

            services.AddMongoDb(Configuration);
            services.AddTransient<IDatabaseSeeder, CustomMongoSeeder>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
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

            app.ApplicationServices.GetService<IDatabaseInitializer>().InitializeAsync();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers(); //.RequireCors(MyAllowSpecificOrigins);
            });
            //app.UseMvc();
        }
    }
}
