# GatewayBrickWise

GatewayBrickWise es una pasarela (gateway) construida en .NET 8 que utiliza YARP (Yet Another Reverse Proxy) para enrutar solicitudes HTTP y aplicar autenticación basada en JWT. Este proyecto está diseñado para servir como punto de entrada seguro y flexible para microservicios o APIs internas.

## Características principales
- **Reverse Proxy con YARP**: enruta solicitudes a servicios backend según configuración.
- **Autenticación JWT**: valida tokens JWT en las cabeceras Authorization.
- **Middlewares personalizados**:
  - `AuthorizationMiddleware`: verifica la autenticación y permite rutas públicas según configuración.
  - `PublicRouteBypassMiddleware`: permite omitir autenticación en rutas públicas.
- **Configuración flexible**: mediante archivos `appsettings.json` y variables de entorno.

## Estructura del proyecto
- `Program.cs`: punto de entrada, configuración de middlewares y YARP.
- `AuthorizationMiddleware.cs`: lógica de autorización personalizada.
- `PublicRouteBypassMiddleware.cs`: lógica para rutas públicas.
- `JwtExtensions.cs`: utilidades para manejo de JWT.
- `appsettings.json`: configuración de rutas, autenticación y servicios backend.

## Instalación y ejecución
1. **Requisitos previos**:
   - .NET 8 SDK
2. **Restaurar dependencias**:
   ```bash
   dotnet restore
   ```
3. **Ejecutar en modo desarrollo**:
   ```bash
   dotnet run
   ```
4. **Compilar para producción**:
   ```bash
   dotnet publish -c Release -r linux-x64 --self-contained false
   ```

## Configuración
- Edita `appsettings.json` para definir rutas, servicios backend y parámetros de autenticación.
- Usa `appsettings.Development.json` para configuraciones locales.

## Ejemplo de cabecera Authorization
```
Authorization: Bearer <token_jwt>
```

## Personalización de rutas públicas
Agrega la clave `SkipAuth` en el contexto de la solicitud para omitir la autenticación en rutas específicas (ver `PublicRouteBypassMiddleware`).

## Ejemplo: agregar una API al gateway
Para agregar una nueva API gestionada por el gateway, edita el archivo `appsettings.json` agregando un nuevo cluster y una nueva ruta en la sección de YARP. Ejemplo:

```json
"ReverseProxy": {
  "Clusters": {
    "miApi": {
      "Destinations": {
        "miApiDestino": {
          "Address": "https://url-de-tu-api/"
        }
      }
    }
  },
  "Routes": [
    {
      "RouteId": "miApiRoute",
      "ClusterId": "miApi",
      "Match": {
        "Path": "/mi-api/{**catch-all}"
      }
    }
  ]
}
```

Con esto, todas las solicitudes que lleguen a `/mi-api/*` serán redirigidas al backend configurado en `Address`.
## Licencia
MIT
