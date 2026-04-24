# Guía Rápida: API RESTful en .NET 10 (Linux + VS Code)

Hermano, acá tenés todos los comandos que necesitás para armar una API RESTful bien estructurada, con capas separadas (Class Libraries) y Entity Framework Core. Todo listo para correr en Linux con VS Code, sin Visual Studio.

## Prerrequisito

Verificá que tengas el SDK de .NET instalado (ya tenés el 10.0.107, ¡estás bien!):

```bash
dotnet --version
```

---

## Configuración para Linux: Límite de inotify (recomendado)

Si vas a usar `dotnet watch` (hot reload) en Linux, puede aparecer un error de límite de instancias inotify. Esto se soluciona aumentando el límite del sistema de forma permanente con estas líneas:

```bash
echo "fs.inotify.max_user_instances=512" | sudo tee -a /etc/sysctl.conf
sudo sysctl -p
```

**¿Qué hacen?**

- La primera línea agrega la configuración `fs.inotify.max_user_instances=512` al archivo `/etc/sysctl.conf`. Esto sube el límite de instancias inotify de 128 a 512, necesario para que .NET pueda monitorear todos los archivos del proyecto.
- La segunda línea aplica los cambios inmediatamente sin necesidad de reiniciar el sistema.

---

## 1. Comandos con explicación paso a paso

### Paso 1: Crear la Solución (contenedor de proyectos)

```bash
dotnet new sln -n MiApiSolution
```

**¿Qué hace?** Crea el archivo `.sln` que agrupa todos los proyectos. Es el "directorio raíz" de tu solución, para que no tengas proyectos sueltos.

---

### Paso 2: Crear Class Libraries (capas de negocio)

Estas librerías no tienen interfaz, son solo código organizado por responsabilidad:

#### Capa de Dominio (entidades puras)

```bash
dotnet new classlib -n MiApi.Domain
```

**¿Qué hace?** Acá van tus entidades (ej. `Producto.cs`), interfaces y reglas de negocio puras. Sin dependencias de bases de datos ni frameworks. Es el corazón de tu app.

#### Capa de Aplicación (casos de uso y DTOs)

```bash
dotnet new classlib -n MiApi.Application
```

**¿Qué hace?** Casos de uso, DTOs (objetos que viajan por la API), y lógica de aplicación. Depende del Dominio.

#### Capa de Infraestructura (base de datos y externos)

```bash
dotnet new classlib -n MiApi.Infrastructure
```

**¿Qué hace?** Implementaciones de bases de datos, EF Core, servicios externos. Es la capa que toca el "mundo exterior".

---

### Paso 3: Crear el proyecto de la API (punto de entrada)

¡Ojo! Usamos `webapi`, no `mvc` (el MVC clásico viene con vistas HTML, para REST no lo necesitás):

```bash
dotnet new webapi -n MiApi.Api --no-https
```

**¿Qué hace?** Crea el proyecto que expone los endpoints RESTful. El flag `--no-https` evita problemas con certificados en Linux local. Si querés HTTPS, sacalo.

---

### Paso 4: Agregar Entity Framework Core

Para que la capa de Infraestructura pueda usar EF Core (ORM para base de datos):

```bash
# Proveedor de SQL Server (cambiá por SQLite/PostgreSQL si usás otro)
dotnet add MiApi.Infrastructure/ package Microsoft.EntityFrameworkCore.SqlServer
# Herramientas para migraciones
dotnet add MiApi.Infrastructure/ package Microsoft.EntityFrameworkCore.Design
```

### Paso 5: Agregar Scalar UI (Reemplazo moderno de Swagger)

```bash
dotnet add MiApi.Api/ package Scalar.AspNetCore
```

**¿Qué hace?** Instala Scalar, una interfaz de documentación de API moderna, rápida y mucho más legible que Swagger UI. Se integra con el OpenAPI que ya viene en .NET 10.

---

### Paso 5: Linkear proyectos (referencias y solución)

#### Agregar todos los proyectos a la solución

```bash
dotnet sln add MiApi.Domain/MiApi.Domain.csproj
dotnet sln add MiApi.Application/MiApi.Application.csproj
dotnet sln add MiApi.Infrastructure/MiApi.Infrastructure.csproj
dotnet sln add MiApi.Api/MiApi.Api.csproj
```

#### Agregar dependencias entre capas (fundamental para la arquitectura limpia)

```bash
# Application necesita Domain
dotnet add MiApi.Application/ reference MiApi.Domain/

# Infrastructure necesita Domain y Application
dotnet add MiApi.Infrastructure/ reference MiApi.Domain/
dotnet add MiApi.Infrastructure/ reference MiApi.Application/

# Api necesita Application e Infrastructure
dotnet add MiApi.Api/ reference MiApi.Application/
dotnet add MiApi.Api/ reference MiApi.Infrastructure/
```

---

### Paso 6: Abrir en VS Code

```bash
cd MiApi.Api && code .
```

---

### Paso 7: Levantar la App con Hot Reload (dotnet watch)

Para correr la API con recarga automática al guardar cambios (hot reload), **tenés que estar dentro de la carpeta de la API**. Si usaste el script con `PROJECT_NAME="test"`, tu carpeta de API es `test.Api` (no la genérica `MiApi.Api`):

```bash
# Entrar a la carpeta correcta de tu API
cd test.Api  # Cambiá por el nombre de tu API si es distinto
# Levantar con hot reload
dotnet watch
```

**¿Qué hace?**

- `dotnet watch` monitorea cambios en los archivos del proyecto y reinicia la app automáticamente.
- Al compilarse, la terminal mostrará el puerto donde escucha la app (ej. `Now listening on: http://localhost:5275`).
- Abrí en tu navegador la URL `http://localhost:XXXX/scalar/v1` (reemplazá `XXXX` por el puerto que te indique la terminal) para ver la interfaz moderna de Scalar.

---

## 2. Script todo-en-uno (ejecutar todo de una vez)

Copiá y pegá este bloque en tu terminal, cambiá el nombre del proyecto si querés (variable `PROJECT_NAME`):

```bash
#!/bin/bash

# Cambiá este nombre si querés otro
PROJECT_NAME="MiApi"

# Crear solución
dotnet new sln -n ${PROJECT_NAME}Solution

# Crear Class Libraries
dotnet new classlib -n ${PROJECT_NAME}.Domain
dotnet new classlib -n ${PROJECT_NAME}.Application
dotnet new classlib -n ${PROJECT_NAME}.Infrastructure

# Crear API
dotnet new webapi -n ${PROJECT_NAME}.Api --no-https

# Agregar EF Core a Infraestructura
dotnet add ${PROJECT_NAME}.Infrastructure/ package Microsoft.EntityFrameworkCore.SqlServer
dotnet add ${PROJECT_NAME}.Infrastructure/ package Microsoft.EntityFrameworkCore.Design

# Agregar Scalar UI a la API (reemplazo moderno de Swagger)
dotnet add ${PROJECT_NAME}.Api/ package Scalar.AspNetCore

# Agregar proyectos a la solución
dotnet sln add ${PROJECT_NAME}.Domain/${PROJECT_NAME}.Domain.csproj
dotnet sln add ${PROJECT_NAME}.Application/${PROJECT_NAME}.Application.csproj
dotnet sln add ${PROJECT_NAME}.Infrastructure/${PROJECT_NAME}.Infrastructure.csproj
dotnet sln add ${PROJECT_NAME}.Api/${PROJECT_NAME}.Api.csproj

# Agregar referencias entre capas
dotnet add ${PROJECT_NAME}.Application/ reference ${PROJECT_NAME}.Domain/
dotnet add ${PROJECT_NAME}.Infrastructure/ reference ${PROJECT_NAME}.Domain/
dotnet add ${PROJECT_NAME}.Infrastructure/ reference ${PROJECT_NAME}.Application/
dotnet add ${PROJECT_NAME}.Api/ reference ${PROJECT_NAME}.Application/
dotnet add ${PROJECT_NAME}.Api/ reference ${PROJECT_NAME}.Infrastructure/

# --- Estructura de Carpetas Hexagonal Pura ---
# Domain (Puertos e Entidades)
mkdir -p ${PROJECT_NAME}.Domain/Entities
mkdir -p ${PROJECT_NAME}.Domain/Ports
mkdir -p ${PROJECT_NAME}.Domain/ValueObjects
mkdir -p ${PROJECT_NAME}.Domain/Exceptions

# Application (CQRS y Mappers)
mkdir -p ${PROJECT_NAME}.Application/DTOs
mkdir -p ${PROJECT_NAME}.Application/Mappers
mkdir -p ${PROJECT_NAME}.Application/Commands
mkdir -p ${PROJECT_NAME}.Application/Queries
mkdir -p ${PROJECT_NAME}.Application/Validators

# Infrastructure (Adaptadores y Datos)
mkdir -p ${PROJECT_NAME}.Infrastructure/Data/Configurations
mkdir -p ${PROJECT_NAME}.Infrastructure/Adapters
mkdir -p ${PROJECT_NAME}.Infrastructure/Services

# Api (Controladores)
mkdir -p ${PROJECT_NAME}.Api/Controllers

# --- Limpieza de la API: De Minimal API a Controllers ---
# 1. Reescribir Program.cs para usar Controllers y Scalar (sin lógica de negocio)
cat > ${PROJECT_NAME}.Api/Program.cs << 'EOF'
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();
EOF

# 2. Crear el Controller de ejemplo (WeatherForecast) en su lugar correcto
cat > ${PROJECT_NAME}.Api/Controllers/WeatherForecastController.cs << 'EOF'
using Microsoft.AspNetCore.Mvc;

namespace ${PROJECT_NAME}.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    [HttpGet]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    }
}

public class WeatherForecast
{
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string? Summary { get; set; }
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
EOF

echo "¡Listo! Abrí VS Code con: cd ${PROJECT_NAME}.Api && code ."
```

---

## Notas importantes

- Si usás otra base de datos (ej. PostgreSQL), reemplazá `SqlServer` por `Npgsql` en el paquete de EF Core.
- La estructura de capas (Domain/Application/Infrastructure) es Clean Architecture básica: te va a ahorrar dolores de cabeza después cuando la app crezca.
- ¿Se entiende? ¡Dale, probalo y me contás!
