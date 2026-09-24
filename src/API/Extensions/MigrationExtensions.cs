using Infrastructure.DataBase.Context;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class MigrationExtensions
{
    public static async Task<IHost> ApplyMigrationsAsync(this IHost host)
    {
        using var scope = host.Services.CreateScope();
        var services = scope.ServiceProvider;

        try
        {
            var context = services.GetRequiredService<AppDbContext>();
            // Создаёт БД, если её нет, и применяет все незавершённые миграции.
            await context.Database.MigrateAsync();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "Произошла ошибка при применении миграций базы данных.");
            throw;
        }

        return host;
    }
}
