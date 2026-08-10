# ApiBrowkeLab

Descripción
ApiBrowkeLab es una API REST desarrollada con .NET 10 que expone servicios para gestionar recursos del dominio (modelos, usuarios y lógica asociada). Actualmente contiene la solución principal, modelos básicos (por ejemplo Models/User.cs) y la estructura inicial de la API (Controllers, Services, Data/ Persistence). Está pensada para ser consumida por clientes HTTP (frontends, móviles o servicios) y evolucionar con autenticación, validaciones y persistencia mediante EF Core.


Componentes principales
- Solución (.slnx): orquesta los proyectos que componen la aplicación.
- Api (proyecto principal): contiene Program.cs, configuración, middlewares y los controladores HTTP.
- Models/: entidades y DTOs (por ejemplo Models/User.cs).
- Controllers/: endpoints que exponen la API REST.
- Services/: lógica de negocio y reglas de aplicación.
- Data / Persistence: DbContext, repositorios y migraciones EF Core (la API está desarrollada usando EF Core).
- Migrations/: migraciones de la base de datos generadas por EF Core.
- Configuración: appsettings.json y Properties/launchSettings.json para entornos y lanzamiento.
- Tests/: (pendiente) pruebas unitarias e integración.


Qué hace la API (estado actual)
- La API está enfocada en la gestión de usuarios y, en el estado actual, expone dos endpoints principales que ofrecen los servicios básicos de gestión de usuarios:

- Registro de usuarios (signup): crea una cuenta de usuario y la persiste en la base de datos vía EF Core.
- Autenticación / Login: valida credenciales y devuelve un token/identificador de sesión (según implementación).

La persistencia ya está implementada con EF Core (DbContext y migraciones presentes) y los endpoints usan la capa de datos para crear y validar usuarios.

Requisitos para replicarlo (pasos para quien clone el repositorio)
1. Prerrequisitos		
   - .NET SDK 10.x instalado: https://dotnet.microsoft.com
   - (Opcional) Visual Studio 2022/2026 o VS Code con extensiones C#.
   - Base de datos: SQL Server (la API está configurada para usar EF Core con SQL Server). Si no se quiere usar SQL Server localmente, se puede ajustar la cadena de conexión y migraciones según se necesite.

2. Clonar el repositorio
   git clone <url>
   cd ApiBrowkeLab

3. Añadir archivos de configuración locales
   - Crear appsettings.Development.json o configurar variables de entorno para la cadena de conexión (ConnectionStrings:DefaultConnection) y otros secretos.
   - No incluir secretos en el repositorio; .gitignore ya excluye appsettings.Development.json.

4. Restaurar y compilar
   dotnet restore
   dotnet build

5. Preparar la base de datos
   - La API utiliza EF Core. Si necesitas aplicar migraciones existentes:
	 dotnet ef database update --project <Proyecto.Data> --startup-project <Proyecto.API>
   - Si quieres generar nuevas migraciones:
	 dotnet ef migrations add <Nombre> --project <Proyecto.Data> --startup-project <Proyecto.API>

6. Ejecutar la API
   - Desde la carpeta del proyecto API:
	 dotnet run
   - O abrir ApiBrowkeLab.slnx en Visual Studio y ejecutar F5.

7. Acceder y probar
   - La API escuchará en el puerto configurado (ver launchSettings.json o salida de dotnet run).
   - Recomendada: añadir e integrar Swagger/OpenAPI para descubrir endpoints.

8. Ejemplos de request/response (registro y login)

La API expone los endpoints bajo el controlador AuthController con prefijo /api/auth.
Todos los requests esperan Content-Type: application/json.

- Registro (POST /api/auth/register)
  - Request JSON (se ajusta al DTO RegisterRequest):
	{
	  "username": "ejemplo",
	  "email": "usuario@ejemplo.com",
	  "password": "P@ssw0rd"
	}
  - Respuesta éxito (200 OK): devuelve un AuthResponse con información del registro
	{
	  "success": true,
	  "message": "Usuario registrado correctamente",
	  "username": "ejemplo",
	  "email": "usuario@ejemplo.com"
	}
  - Respuesta error (400 Bad Request): devuelve AuthResponse con success = false y mensaje explicando el problema.

- Login (POST /api/auth/login)
  - Request JSON (se ajusta al DTO LoginRequest):
	{
	  "email": "usuario@ejemplo.com",
	  "password": "P@ssw0rd"
	}
  - Respuesta éxito (200 OK): devuelve únicamente el nombre de usuario asociado
	{
	  "username": "ejemplo"
	}
  - Respuesta error (401 Unauthorized): devuelve AuthResponse con success = false y mensaje de error.

Notas
- Las rutas exactas son: POST /api/auth/register y POST /api/auth/login (definidas en ApiBrowkeLab/Controllers/AuthController.cs).
- Los DTOs están en ApiBrowkeLab/DTOs: RegisterRequest (username, email, password), LoginRequest (email, password) y AuthResponse (success, message, username, email).
- Ajusta los ejemplos si cambias la implementación de la respuesta (por ejemplo, incluir token JWT en login).

Ejemplos curl

-- Registro (ejemplo)
curl -X POST "https://localhost:5001/api/auth/register" \
  -H "Content-Type: application/json" \
  -d '{"username":"ejemplo","email":"usuario@ejemplo.com","password":"P@ssw0rd"}'

-- Login (ejemplo)
curl -X POST "https://localhost:5001/api/auth/login" \
  -H "Content-Type: application/json" \
  -d '{"email":"usuario@ejemplo.com","password":"P@ssw0rd"}'

Reemplaza el host/puerto (https://localhost:5001) por el que use tu entorno (ver launchSettings.json o salida de dotnet run).
