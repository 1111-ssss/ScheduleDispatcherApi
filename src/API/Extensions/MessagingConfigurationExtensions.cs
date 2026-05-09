using MassTransit;

namespace API.Extensions;

public static class MessagingConfigurationExtensions
{
    public static IServiceCollection AddMessaging(this IServiceCollection service, IConfiguration conf)
    {

        service.AddMassTransit(
            x => x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(conf["RabbitMq:Host"] ?? "localhost", "/");
            })
            );


        return service;
    }
}
