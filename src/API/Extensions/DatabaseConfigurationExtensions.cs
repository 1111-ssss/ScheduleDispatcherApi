using Application.Abstractions.Interfaces.UnitOfWork;
using Application.Abstractions.Repository.Base;
using Application.Abstractions.Repository.Custom;
using Infrastructure.DataBase.Context;
using Infrastructure.DataBase.Repository.Base;
using Infrastructure.DataBase.Repository.Custom;
using Infrastructure.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace API.Extensions;

public static class DatabaseConfigurationExtensions
{
    public static IServiceCollection AddCustomDbContext(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection"))
        );

        // UnitOfWork
        services.AddScoped<IUnitOfWork, UnitOfWork>();

        // Repository
        services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));

        services.AddScoped<ISubjectRepository, SubjectRepository>();

        return services;
    }
}