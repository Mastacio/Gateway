using GatewayBrickWise;

var builder = WebApplication.CreateBuilder(args);
var config = builder.Configuration;

// Add services
builder.Services.AddJwt(config);
builder.Services.AddAuthorization();
builder.Services.AddReverseProxy().LoadFromConfig(config.GetSection("ReverseProxy")) .AddTransforms(builderContext =>
{
    // Por si quieres modificar headers o cosas globales
    
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", policy =>
    {
        policy
            .WithOrigins(config.GetSection("Cors:Origins").Get<string[]>() ?? new string[] { })
            .AllowAnyHeader()
            .AllowAnyMethod();
        // .AllowCredentials(); // si usas cookies o auth headers
    });
});

var app = builder.Build();

app.UseCors("CorsPolicy");

// Middleware para identificar rutas públicas y marcar SkipAuth
app.UsePublicRouteBypass();

// Autenticación y autorización global
app.UseAuthentication();
app.UseMiddleware<AuthorizationMiddleware>();
app.UseAuthorization();

// Middleware condicional para bloquear acceso a rutas no públicas

// Redirección al proxy
app.MapReverseProxy();

app.Run();