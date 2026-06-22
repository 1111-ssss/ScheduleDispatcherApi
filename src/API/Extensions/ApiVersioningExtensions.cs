namespace API.Extensions;

public static class ApiVersioningExtensions
    {
    public const string CurrentVersion = "1.0";
    public const string HeaderName = "X-Api-Version";

    private static readonly HashSet<string> SupportedVersions =
        new(StringComparer.OrdinalIgnoreCase) { "1", "1.0" };

    public static WebApplication UseApiVersioning(this WebApplication app)
    {
        app.Use(async (context, next) =>
        {
            if (context.Request.Headers.TryGetValue(HeaderName, out var requested)
                && !SupportedVersions.Contains(requested.ToString().Trim()))
            {
                context.Response.StatusCode = StatusCodes.Status400BadRequest;
                await context.Response.WriteAsJsonAsync(new
                {
                    error = "UnsupportedApiVersion",
                    message = $"Неподдерживаемая версия API '{requested}'. Поддерживается: {CurrentVersion}."
                });
                return;
            }

            context.Response.OnStarting(() =>
            {
                context.Response.Headers[HeaderName] = CurrentVersion;
                return Task.CompletedTask;
            });

            await next();
        });

        return app;
    }
            }
