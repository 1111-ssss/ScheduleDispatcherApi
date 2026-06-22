using System.Threading.RateLimiting;

namespace API.Extensions;

public static class RateLimitExtension
    {
    public static IServiceCollection AddRateLimit(this IServiceCollection services)
    {
        services.AddRateLimiter(options =>
        {

            options.AddPolicy("DefaultLimiter", context =>
            {
                return RateLimitPartition.GetTokenBucketLimiter(GetUserKey(context), _ => new TokenBucketRateLimiterOptions
                    {
                        TokenLimit = 200,
                        ReplenishmentPeriod = TimeSpan.FromSeconds(10),
                        TokensPerPeriod = 100,
                        AutoReplenishment = true,
                        QueueProcessingOrder = QueueProcessingOrder.OldestFirst,
                        QueueLimit = 20
                    });
            });

            options.AddPolicy("StrictLimiter", context =>
            {
                return RateLimitPartition.GetFixedWindowLimiter(GetUserKey(context), _ => new FixedWindowRateLimiterOptions
                    {
                        AutoReplenishment = true,
                        PermitLimit = 5,
                        Window = TimeSpan.FromSeconds(10)
                    });
            });

            options.GlobalLimiter = PartitionedRateLimiter.Create<HttpContext, string>(context =>
            {

                return RateLimitPartition.GetTokenBucketLimiter(GetUserKey(context), _ => new TokenBucketRateLimiterOptions
                {
                    TokenLimit = 1000,
                    ReplenishmentPeriod = TimeSpan.FromMinutes(1),
                    TokensPerPeriod = 600,
                    AutoReplenishment = true,
                    QueueLimit = 100,
                    QueueProcessingOrder = QueueProcessingOrder.OldestFirst
                });
            });

            options.OnRejected = async (context, token) =>
            {
                context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
            };
        });
        return services;
    }
    public static string GetUserKey(HttpContext context)
    {
        return context.User.Identity?.IsAuthenticated == true
            ? $"user:{context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value}"
            : $"ip:{context.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";
    }
    }
