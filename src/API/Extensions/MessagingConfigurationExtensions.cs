using MassTransit;

namespace API.Extensions;

public static class MessagingConfigurationExtensions
    {
    public static IServiceCollection AddMessaging(this IServiceCollection service, IConfiguration conf)
    {
        var host = conf["RabbitMq:Host"] ?? "localhost";
        if (BrokerReachable(host))
        {
            service.AddMassTransit(
                x => x.UsingRabbitMq((context, cfg) =>
                {
                    cfg.Host(host, "/");
                })
                );
        }
        else
        {

            Console.WriteLine($"WARN: RabbitMQ на {host} недоступен — шина in-memory (finalize никуда не публикуется).");
            service.AddMassTransit(
                x => x.UsingInMemory((context, cfg) =>
                {
                    cfg.ConfigureEndpoints(context);
                })
                );
        }

        return service;
    }

    private static bool BrokerReachable(string host)
    {
        try
        {
            using var client = new System.Net.Sockets.TcpClient();
            var task = client.ConnectAsync(host, 5672);
            return task.Wait(TimeSpan.FromSeconds(3)) && client.Connected;
        }
        catch
        {
            return false;
        }
    }
        }
