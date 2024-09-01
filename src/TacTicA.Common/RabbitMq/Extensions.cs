using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MassTransit;

namespace TacTicA.Common.RabbitMq
{
    public static class Extensions
    {
        /// <summary>
        /// Resolves Queue name.
        /// Using slash in the name - it's usual (per author of Udemy).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static string GetQueueName<T>()
            => $"{Assembly.GetEntryAssembly().GetName()}/{typeof(T).Name}";

        /// <summary>
        /// Resolve rabbitMQ client.
        /// </summary>
        /// <param name="services">Default IoC engine.</param>
        /// <param name="configuration"></param>
        /// <param name="addConsumers"></param>
        public static void AddRabbitMq(this IServiceCollection services, IConfiguration configuration, Action<IBusRegistrationConfigurator> addConsumers)
        {
            var options = new RabbitMqOptions();
            var section = configuration.GetSection("rabbitmq");
            section.Bind(options);

            services.AddMassTransit(busConfigurator =>
            {
                addConsumers(busConfigurator);
                
                busConfigurator.UsingRabbitMq((context, configurator) =>
                {
                    configurator.Host(options.Hostnames[0], (ushort)options.Port, options.VirtualHost,
                        h =>
                        {
                            h.Username(options.Username);
                            h.Password(options.Password);
                        });

                    configurator.ConfigureEndpoints(context);
                });
            });
        }
    }
}