using Scalar.AspNetCore;

namespace API.Extensions;

public static class MiddlewareConfigurationExtensions
{
    public static WebApplication UseCustomMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference(options =>
            {
                options
                    .WithTitle("Schedule Dispatcher API")
                    .WithTheme(ScalarTheme.Moon)
                    .WithDefaultHttpClient(ScalarTarget.CSharp, ScalarClient.Fetch)
                    .AddPreferredSecuritySchemes("Bearer")
                    .AddHttpAuthentication("Bearer", auth =>
                    {
                        auth.Description = "Тут токен Bearer";
                    });
                options.Agent?.Disabled = true;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();

        return app;
    }
}