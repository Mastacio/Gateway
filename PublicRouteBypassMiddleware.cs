namespace GatewayBrickWise;

public class PublicRouteBypassMiddleware(RequestDelegate next, IConfiguration configurationBuilder)
{
    private readonly string[] _publicRoutes = configurationBuilder.GetSection("PublicRoutes").Get<string[]>() ?? [];

    public async Task InvokeAsync(HttpContext context)
    {
        // Obtener la ruta después de que el PathBase /api sea manejado por UsePathBase
        var path = context.Request.Path.ToString().ToLower();
        
        // También verificar la ruta original para compatibilidad
        var originalPath = context.Request.PathBase + path;
        
        if (_publicRoutes.Any(route => path.StartsWith(route) || originalPath.StartsWith(route)))
        {
            // Saltar autenticación
            Console.WriteLine($"✅ Public route match: {path} (Original: {originalPath})");
            context.Items["SkipAuth"] = true;
        }

        await next(context);
    }
}

public static class PublicRouteBypassMiddlewareExtensions
{
    public static IApplicationBuilder UsePublicRouteBypass(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<PublicRouteBypassMiddleware>();
    }
}