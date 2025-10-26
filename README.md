# Proyecto Aspire - Desarrollo Web 2025

Este repositorio contiene el desarrollo del proyecto de la materia de Desarrollo Web utilizando arquitectura de microservicios con .NET Aspire.

**Desarrollado por:** Johan Cala
**Universidad de Almería - 2025**

---

## 📋 Descripción

Proyecto de microservicios desarrollado con .NET Aspire 9 para la asignatura de Desarrollo Web. Implementa una arquitectura moderna basada en contenedores con orquestación, telemetría y service discovery integrados.

---

## 🛠️ Tecnologías

- **.NET 9.0** - Framework principal
- **.NET Aspire 9.5** - Orquestación de microservicios
- **C#** - Lenguaje de programación
- **Docker** - Contenedores
- **Entity Framework Core 9.0** - ORM para bases de datos
- **Blazor** - Frontend (opcional)
- **ASP.NET Core** - APIs REST

---

## 📦 Requerimientos del Sistema

### Software Necesario:

| Herramienta | Versión Mínima | Versión Recomendada | Propósito |
|-------------|----------------|---------------------|-----------|
| **.NET SDK** | 9.0.0 | 9.0.306+ | Desarrollo y compilación |
| **Docker Desktop** | 20.10+ | 28.5+ | Contenedores |
| **Git** | 2.30+ | 2.45+ | Control de versiones |
| **Visual Studio Code** | 1.80+ | 1.105+ | Editor de código |
| **GitHub CLI** | 2.0+ | 2.76+ | Gestión de repositorio |

### Workloads y Herramientas de .NET:

```bash
# Verificar instalación
dotnet --version                    # Debe ser 9.0.306 o superior
dotnet workload list                # Debe incluir 'aspire'
docker --version                    # Debe estar instalado
git --version                       # Debe estar instalado
```

---

## 🚀 Instalación y Configuración

### 1. Clonar el Repositorio

```bash
git clone https://github.com/jct576/ProyectoAspireMDAW2025.git
cd ProyectoAspireMDAW2025
```

### 2. Instalar .NET Aspire Workload

```bash
# Actualizar workloads
dotnet workload update

# Instalar Aspire (si no está instalado)
dotnet workload install aspire
```

### 3. Instalar Herramientas Globales

```bash
# Aspire CLI
dotnet tool install -g Aspire.Cli --prerelease

# Entity Framework Core Tools
dotnet tool install --global dotnet-ef

# User Secrets (para desarrollo)
dotnet tool install --global dotnet-user-secrets
```

### 4. Instalar Plantillas de Aspire

```bash
dotnet new install Aspire.ProjectTemplates --force
```

### 5. Verificar Docker

```bash
# Iniciar Docker Desktop
# Verificar que esté corriendo
docker ps
```

---

## 🏗️ Estructura del Proyecto

```
ProyectoAspireMDAW2025/
├── src/                          # Código fuente
│   ├── AppHost/                  # Orquestador de Aspire
│   ├── ServiceDefaults/          # Configuración compartida
│   └── Services/                 # Microservicios
├── tests/                        # Pruebas unitarias e integración
├── docs/                         # Documentación
├── .gitignore                    # Archivos ignorados por Git
├── GITFLOW.md                    # Metodología Git Flow
└── README.md                     # Este archivo
```

---

## 🌳 Metodología Git Flow

Este proyecto utiliza **Git Flow** con las siguientes ramas:

- **`master`** - Producción (código estable)
- **`staging`** - Pre-producción/QA
- **`develop`** - Desarrollo principal ⭐ **(rama actual)**
- **`feature/*`** - Nuevas funcionalidades
- **`bugfix/*`** - Corrección de bugs
- **`hotfix/*`** - Correcciones urgentes

### Workflow de Desarrollo:

```bash
# 1. Crear feature branch desde develop
git checkout develop
git checkout -b feature/mi-funcionalidad

# 2. Desarrollar y hacer commits
git add .
git commit -m "feat: Descripción del cambio" \
           -m "Co-authored-by: Johan Cala <JohanCalaT@users.noreply.github.com>"

# 3. Merge a develop
git checkout develop
git merge feature/mi-funcionalidad
git push origin develop
```

Ver [GITFLOW.md](GITFLOW.md) para más detalles.

---

## 🔧 Comandos Útiles

### Desarrollo:

```bash
# Restaurar dependencias
dotnet restore

# Compilar proyecto
dotnet build

# Ejecutar proyecto
dotnet run --project src/AppHost

# Ejecutar tests
dotnet test
```

### Docker:

```bash
# Ver contenedores corriendo
docker ps

# Ver logs de un contenedor
docker logs <container-id>

# Limpiar contenedores
docker compose down
```

### Entity Framework:

```bash
# Crear migración
dotnet ef migrations add NombreMigracion

# Aplicar migraciones
dotnet ef database update

# Revertir migración
dotnet ef database update MigracionAnterior
```

---

## 📚 Documentación Adicional

- [GITFLOW.md](GITFLOW.md) - Metodología completa de Git Flow
- [Documentación oficial de .NET Aspire](https://learn.microsoft.com/en-us/dotnet/aspire/)
- [Guía de Entity Framework Core](https://learn.microsoft.com/en-us/ef/core/)

---

## 👨‍💻 Autor

**Johan Cala**
- GitHub Universidad: [@jct576](https://github.com/jct576)
- GitHub Personal: [@JohanCalaT](https://github.com/JohanCalaT)
- Email: jct576@inlumine.ual.es

---

## 📄 Licencia

Este proyecto es parte de un trabajo académico para la Universidad de Almería.

---

*Última actualización: Octubre 2025*

