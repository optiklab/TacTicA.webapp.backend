using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using TacTicA.Common.Logging;
using TacTicA.Common.OpenApi;

namespace TacTicA.Services.Cities
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "TacTicAPolicy";
    
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; private set; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddHttpClient();
            services.AddCors(options =>
            {
                options.AddPolicy(name: MyAllowSpecificOrigins,
                    builder =>
                    {
                        builder
                            .AllowAnyOrigin()
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                        //builder.WithOrigins(
                        //    //"https://tacticaddon.herokuapp.com",
                        //    //"https://2f4956d8846b.ngrok.io",
                        //    "https://*.tactica.xyz",
                        //    "https://tactica.xyz",
                        //    "https://tacticapi-87817175.us-east-1.elb.amazonaws.com")
                        //        .AllowAnyHeader()
                        //        .AllowAnyMethod();
                    });
            });
            services.AddMemoryCache();
            services.AddControllers();
            services.AddLogging();

            services.AddNLogLogging();
            services.AddRawHttpRequestResponseLogging();

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
                app.UseSwaggerUI(); // http://localhost:5010/swagger/index.html
            }
            else
            {
                // I'm not using any of 443/SSL/HTTPS configuration parameters because I'm using reverse-proxy in front of kestrel server
                // to control certificates and SSL connection.
                //// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                //app.UseHsts();
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseCors(MyAllowSpecificOrigins);

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints
                    .MapControllers()
                    .RequireCors(MyAllowSpecificOrigins);
            });
        }
    }
}
