using API.Endpoints;

namespace API.Extensions;

public static class RoutingExtensions
{
    public static void MapCustomRoutes(this WebApplication app)
    {
        app.MapAuthEndpoints();
    }
}