# Fase 0-1: Proyectos Compartidos

## 📋 Resumen

Esta fase implementa los **proyectos compartidos** que serán utilizados por todos los microservicios del sistema. Estos proyectos contienen código reutilizable para eventos, DTOs, abstracciones de mensajería y observabilidad.

---

## 🏗️ Estructura Creada

```
src/Shared/
├── ProyectoAspireMDAW2025.Contracts/
│   └── Events/
│       ├── BaseEvent.cs
│       ├── Auth/
│       │   ├── UserRegisteredEvent.cs
│       │   ├── UserLoggedInEvent.cs
│       │   └── TokenRevokedEvent.cs
│       └── User/
│           ├── UserUpdatedEvent.cs
│           └── UserDeletedEvent.cs
│
├── ProyectoAspireMDAW2025.Common/
│   ├── DTOs/
│   │   ├── Requests/Auth/
│   │   │   ├── LoginRequest.cs
│   │   │   ├── RegisterRequest.cs
│   │   │   └── RefreshTokenRequest.cs
│   │   └── Responses/Auth/
│   │       └── AuthResponse.cs
│   ├── Enums/
│   │   ├── UserStatus.cs
│   │   └── UserRole.cs
│   └── Constants/
│       ├── EventTopics.cs
│       └── CacheKeys.cs
│
├── ProyectoAspireMDAW2025.Messaging/
│   ├── Abstractions/
│   │   ├── IEventPublisher.cs
│   │   ├── IEventConsumer.cs
│   │   └── IMessageBus.cs
│   ├── Configuration/
│   │   └── RabbitMQSettings.cs
│   └── Models/
│       └── MessageEnvelope.cs
│
└── ProyectoAspireMDAW2025.Observability/
    ├── Logging/
    │   ├── LoggingExtensions.cs
    │   └── CorrelationIdMiddleware.cs
    ├── Tracing/
    │   └── ActivityExtensions.cs
    └── Metrics/
        └── MetricsExtensions.cs
```

---

## 📦 Proyectos Creados

### 1. ProyectoAspireMDAW2025.Contracts

**Propósito:** Definir eventos de integración compartidos entre microservicios.

**Contenido:**
- **BaseEvent.cs**: Clase base para todos los eventos con propiedades comunes:
  - `EventId`: Identificador único del evento
  - `CorrelationId`: Para rastrear flujos completos
  - `OccurredAt`: Timestamp UTC
  - `PublishedBy`: Servicio que publicó el evento
  - `SchemaVersion`: Versión del esquema (para compatibilidad)

- **Eventos de Auth:**
  - `UserRegisteredEvent`: Publicado cuando un usuario se registra
  - `UserLoggedInEvent`: Publicado cuando un usuario inicia sesión
  - `TokenRevokedEvent`: Publicado cuando un token es revocado

- **Eventos de User:**
  - `UserUpdatedEvent`: Publicado cuando se actualiza un usuario
  - `UserDeletedEvent`: Publicado cuando se elimina un usuario

**Dependencias:** Ninguna (proyecto base)

---

### 2. ProyectoAspireMDAW2025.Common

**Propósito:** DTOs, Enums y Constants compartidos.

**Contenido:**
- **DTOs de Requests:**
  - `LoginRequest`: Email, Password, RememberMe
  - `RegisterRequest`: Email, Password, ConfirmPassword, FirstName, LastName
  - `RefreshTokenRequest`: RefreshToken

- **DTOs de Responses:**
  - `AuthResponse`: AccessToken, RefreshToken, TokenType, ExpiresIn, UserId, Email, FullName

- **Enums:**
  - `UserStatus`: Active, Inactive, Suspended, Deleted, PendingVerification
  - `UserRole`: User, Moderator, Administrator, SuperAdmin

- **Constants:**
  - `EventTopics`: Routing keys para RabbitMQ (auth.user.registered, user.updated, etc.)
  - `CacheKeys`: Claves para Redis con helpers (BlacklistedToken, UserProfile, etc.)

**Dependencias:** 
- `System.ComponentModel.DataAnnotations` (para validaciones)

---

### 3. ProyectoAspireMDAW2025.Messaging

**Propósito:** Abstracciones para mensajería asíncrona con RabbitMQ.

**Contenido:**
- **Abstracciones:**
  - `IEventPublisher`: Interfaz para publicar eventos
  - `IEventConsumer<TEvent>`: Interfaz para consumir eventos
  - `IMessageBus`: Interfaz principal que combina publicación y suscripción

- **Configuration:**
  - `RabbitMQSettings`: Configuración de conexión a RabbitMQ (Host, Port, Username, Password, Exchange, etc.)

- **Models:**
  - `MessageEnvelope<TPayload>`: Envoltura para mensajes con metadata (MessageId, CorrelationId, Source, RetryCount)

**Dependencias:**
- Referencia a `ProyectoAspireMDAW2025.Contracts`

---

### 4. ProyectoAspireMDAW2025.Observability

**Propósito:** Logging estructurado, tracing distribuido y métricas.

**Contenido:**
- **Logging:**
  - `LoggingExtensions`: Métodos de extensión para logging consistente:
    - `LogOperationStart`, `LogOperationSuccess`, `LogOperationFailure`
    - `LogEventPublished`, `LogEventReceived`, `LogEventProcessed`
  - `CorrelationIdMiddleware`: Middleware para gestionar CorrelationId en requests HTTP

- **Tracing:**
  - `ActivityExtensions`: Extensiones para OpenTelemetry Activities:
    - `AddCommonTags`, `AddEventTags`, `AddUserTags`
    - `MarkAsSuccess`, `MarkAsError`

- **Metrics:**
  - `MetricsExtensions`: Métricas personalizadas con OpenTelemetry:
    - Contadores: `events.published`, `events.consumed`, `events.failed`
    - Histogramas: `events.processing.duration`, `operation.duration`

**Dependencias:**
- `Microsoft.Extensions.Logging.Abstractions` (9.0.10)
- `Microsoft.AspNetCore.Http.Abstractions` (2.3.0)

---

## ✅ Verificación

Todos los proyectos compilan correctamente:

```bash
✅ ProyectoAspireMDAW2025.Contracts - Build exitoso
✅ ProyectoAspireMDAW2025.Common - Build exitoso
✅ ProyectoAspireMDAW2025.Messaging - Build exitoso
✅ ProyectoAspireMDAW2025.Observability - Build exitoso
```

---

## 🎯 Próximos Pasos

Con la Fase 0-1 completada, ahora podemos proceder a:

- **Fase 2:** Infraestructura en AppHost (Aspire, SQL Server, Redis, RabbitMQ)
- **Fase 3:** Auth Service completo (Clean Architecture + CQRS)
- **Fase 4:** API Gateway (YARP)

---

## 📝 Notas Técnicas

### Principios Aplicados:
- ✅ **Separation of Concerns**: Cada proyecto tiene una responsabilidad clara
- ✅ **Dependency Inversion**: Uso de abstracciones (interfaces) en lugar de implementaciones concretas
- ✅ **Reusabilidad**: Código compartido entre todos los microservicios
- ✅ **Observability First**: Logging, tracing y metrics desde el inicio

### Convenciones:
- Todos los eventos heredan de `BaseEvent`
- Todos los DTOs usan `Data Annotations` para validación
- Todos los consumers implementan `IEventConsumer<TEvent>`
- Todos los publishers usan `IEventPublisher`

---

## 🔗 Referencias

- [Blueprint de Microservicios](.augment/rules/Blueprint-microservices-aspire.md)
- [Arquitectura del Sistema](.augment/rules/architecture.md)
- [Git Flow](GITFLOW.md)

---

**Commit:** `922f878`  
**Rama:** `feature/fase-0-1-shared-projects`  
**Estado:** ✅ Completado y listo para revisión

