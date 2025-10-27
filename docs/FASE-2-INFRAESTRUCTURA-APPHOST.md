# Fase 2: Infraestructura en AppHost

## 📋 Resumen

Esta fase implementa la **infraestructura base** del sistema usando .NET Aspire. Se configuran los servicios de infraestructura (SQL Server, Redis, RabbitMQ) y se prepara el AppHost para orquestar todos los microservicios.

---

## 🏗️ Estructura Creada

```
src/
├── ProyectoAspireMDAW2025.AppHost/
│   ├── AppHost.cs                          # Orquestación de Aspire
│   ├── ProyectoAspireMDAW2025.AppHost.csproj
│   ├── appsettings.json
│   ├── appsettings.Development.json
│   └── Properties/
│       └── launchSettings.json
│
└── ProyectoAspireMDAW2025.ServiceDefaults/
    ├── Extensions.cs                       # Configuración común para todos los servicios
    └── ProyectoAspireMDAW2025.ServiceDefaults.csproj
```

---

## 📦 Proyectos Creados

### 1. ProyectoAspireMDAW2025.AppHost

**Propósito:** Orquestar todos los servicios y la infraestructura usando .NET Aspire.

**Contenido:**
- **AppHost.cs**: Configuración de infraestructura y orquestación
  - SQL Server con 2 bases de datos: `authdb` y `userdb`
  - Redis para caché distribuido
  - RabbitMQ para mensajería asíncrona
  - Configuración de persistencia de datos con volúmenes

**Paquetes NuGet:**
- `Aspire.Hosting.AppHost` (9.5.2)
- `Aspire.Hosting.SqlServer` (9.5.2)
- `Aspire.Hosting.Redis` (9.5.2)
- `Aspire.Hosting.RabbitMQ` (9.5.2)

**Características:**
- ✅ **ContainerLifetime.Persistent**: Los contenedores persisten entre reinicios
- ✅ **WithDataVolume()**: Los datos se persisten en volúmenes de Docker
- ✅ **WithManagementPlugin()**: RabbitMQ incluye UI de administración

---

### 2. ProyectoAspireMDAW2025.ServiceDefaults

**Propósito:** Configuración común compartida por todos los microservicios.

**Contenido:**
- **Extensions.cs**: Métodos de extensión para configuración estándar
  - `AddServiceDefaults()`: Agrega OpenTelemetry, Health Checks, Service Discovery
  - `ConfigureOpenTelemetry()`: Configura logging, metrics y tracing
  - `AddDefaultHealthChecks()`: Agrega health checks básicos
  - `MapDefaultEndpoints()`: Mapea endpoints de health checks

**Características:**
- ✅ **OpenTelemetry**: Logging, Metrics, Tracing automáticos
- ✅ **Service Discovery**: Descubrimiento automático de servicios
- ✅ **Resilience**: Políticas de retry y circuit breaker
- ✅ **Health Checks**: Endpoints `/health` y `/alive`

---

## 🗄️ Infraestructura Configurada

### SQL Server

**Configuración:**
```csharp
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
```

**Bases de Datos:**
- **authdb**: Base de datos para Auth Service
  - Tablas: Users, RefreshTokens, AuditLogs
- **userdb**: Base de datos para User Service
  - Tablas: UserProfiles, UserSettings

**Conexión:**
- Los servicios recibirán la connection string automáticamente vía Service Discovery
- Formato: `Server=localhost;Database=authdb;User Id=sa;Password=...;TrustServerCertificate=True`

---

### Redis

**Configuración:**
```csharp
var redis = builder.AddRedis("cache")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
```

**Usos:**
- Caché de sesiones de usuario
- Blacklist de tokens JWT revocados
- Caché de perfiles de usuario
- SignalR backplane (para Blazor Server)

**Conexión:**
- Los servicios recibirán la connection string automáticamente
- Formato: `localhost:6379`

---

### RabbitMQ

**Configuración:**
```csharp
var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin()
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();
```

**Usos:**
- Publicación de eventos de integración
- Comunicación asíncrona entre microservicios
- Event-driven architecture

**Acceso:**
- **AMQP**: `localhost:5672` (para aplicaciones)
- **Management UI**: `http://localhost:15672` (usuario: `guest`, password: `guest`)

**Exchanges configurados:**
- `proyectoaspire.events` (topic exchange)
- `proyectoaspire.events.dlx` (dead letter exchange)

---

## 🎯 Características de Aspire

### Service Discovery

Los servicios se descubren automáticamente entre sí:
```csharp
// En Auth Service, para llamar a User Service:
var httpClient = httpClientFactory.CreateClient();
var response = await httpClient.GetAsync("https://userservice/api/users/123");
```

### OpenTelemetry

Telemetría automática para:
- **Logs**: Logs estructurados con contexto
- **Metrics**: Contadores, histogramas, gauges
- **Traces**: Distributed tracing entre servicios

### Health Checks

Endpoints automáticos:
- `/health`: Verifica que todos los health checks pasen
- `/alive`: Verifica solo el health check "live"

### Dashboard de Aspire

Al ejecutar el AppHost, se abre automáticamente el dashboard:
- **URL**: `http://localhost:15000` (puerto puede variar)
- **Características**:
  - Vista de todos los servicios y su estado
  - Logs en tiempo real
  - Traces distribuidos
  - Métricas y gráficos
  - Conexiones entre servicios

---

## ✅ Verificación

### Build Exitoso

```bash
✅ ProyectoAspireMDAW2025.AppHost - Build exitoso (sin advertencias)
✅ ProyectoAspireMDAW2025.ServiceDefaults - Build exitoso
```

### Infraestructura Configurada

- ✅ SQL Server con 2 bases de datos (authdb, userdb)
- ✅ Redis para caché
- ✅ RabbitMQ para mensajería
- ✅ Persistencia de datos con volúmenes
- ✅ Service Discovery configurado
- ✅ OpenTelemetry configurado

---

## 🚀 Próximos Pasos

Con la Fase 2 completada, ahora podemos proceder a:

- **Fase 3:** Auth Service completo (Clean Architecture + CQRS + JWT)
- **Fase 4:** API Gateway (YARP + JWT Validation)
- **Fase 5:** User Service (Clean Architecture + CQRS)
- **Fase 6:** Blazor Web App (Frontend)

---

## 📝 Notas Técnicas

### Persistencia de Datos

Los datos se persisten en volúmenes de Docker:
- **SQL Server**: `/var/opt/mssql`
- **Redis**: `/data`
- **RabbitMQ**: `/var/lib/rabbitmq`

Esto significa que los datos **NO se pierden** al reiniciar los contenedores.

### Puertos Asignados

Aspire asigna puertos automáticamente, pero típicamente:
- **SQL Server**: `localhost:1433`
- **Redis**: `localhost:6379`
- **RabbitMQ AMQP**: `localhost:5672`
- **RabbitMQ Management**: `localhost:15672`
- **Aspire Dashboard**: `localhost:15000` (varía)

### Variables de Entorno

Los servicios reciben automáticamente:
- `ConnectionStrings__authdb`: Connection string de SQL Server
- `ConnectionStrings__cache`: Connection string de Redis
- `ConnectionStrings__messaging`: Connection string de RabbitMQ

---

## 🔗 Referencias

- [.NET Aspire Documentation](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Blueprint de Microservicios](.augment/rules/Blueprint-microservices-aspire.md)
- [Arquitectura del Sistema](.augment/rules/architecture.md)
- [Fase 0-1: Shared Projects](FASE-0-1-SHARED-PROJECTS.md)

---

**Commit:** Pendiente  
**Rama:** `feature/fase-0-1-shared-projects`  
**Estado:** ✅ Completado y listo para commit

