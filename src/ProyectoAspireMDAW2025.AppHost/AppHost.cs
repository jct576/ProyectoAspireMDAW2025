var builder = DistributedApplication.CreateBuilder(args);

// ==================== INFRAESTRUCTURA ====================

// Redis para cache, sessions, SignalR backplane
var redis = builder.AddRedis("cache")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();  // Persistir datos entre reinicios

// SQL Server
var sqlServer = builder.AddSqlServer("sqlserver")
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();  // Persistir datos entre reinicios

// RabbitMQ para mensajería asíncrona
var rabbitmq = builder.AddRabbitMQ("messaging")
    .WithManagementPlugin()  // UI en http://localhost:15672 (guest/guest)
    .WithLifetime(ContainerLifetime.Persistent)
    .WithDataVolume();  // Persistir datos entre reinicios

// ==================== BASES DE DATOS ====================

// Base de datos para Auth Service
var authDb = sqlServer.AddDatabase("authdb");

// Base de datos para User Service
var userDb = sqlServer.AddDatabase("userdb");

// ==================== MICROSERVICIOS ====================

// Auth Service - Autenticación y Autorización
var authApi = builder.AddProject<Projects.ProyectoAspireMDAW2025_Auth_Api>("auth-api")
    .WithReference(authDb)
    .WithReference(redis)
    .WithReference(rabbitmq);

// ==================== NOTA ====================
// Los siguientes microservicios se agregarán en las siguientes fases:
// - Fase 4: API Gateway
// - Fase 5: User Service
// - Fase 6: Blazor Web App

builder.Build().Run();
