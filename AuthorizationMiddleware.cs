namespace GatewayBrickWise
{
    public class AuthorizationMiddleware(RequestDelegate next)
    {
        public async Task InvokeAsync(HttpContext context)
        {
            string? authorizationHeader = context.Request.Headers["Authorization"].FirstOrDefault();
            
            // Imprimir información de diagnóstico para depuración
            Console.WriteLine($"Ruta solicitada: {context.Request.Path}");
            Console.WriteLine($"PathBase: {context.Request.PathBase}");
            Console.WriteLine($"SkipAuth: {context.Items.ContainsKey("SkipAuth")}");

            if (context.Items.ContainsKey("SkipAuth"))
            {
                await next(context);
                return;
            }

            if (context.User.Identity != null && !context.User.Identity.IsAuthenticated)
            {
                await Unauthorized(context);
                return;
            }

            if (string.IsNullOrEmpty(authorizationHeader) || !authorizationHeader.StartsWith("Bearer "))
            {
                await Unauthorized(context);
                return;
            }

            await next(context); // autenticado, puede continuar
        }

        public async Task Unauthorized(HttpContext context)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync("Unauthorized");
            return;
        }
    }
}