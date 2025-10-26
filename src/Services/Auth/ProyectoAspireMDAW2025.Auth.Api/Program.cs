using Microsoft.EntityFrameworkCore;
using ProyectoAspireMDAW2025.Auth.Application;
using ProyectoAspireMDAW2025.Auth.Infrastructure;
using ProyectoAspireMDAW2025.Auth.Infrastructure.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();

// Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Auth Service API",
        Version = "v1",
        Description = "API de autenticación y autorización con ASP.NET Core Identity y JWT",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "ProyectoAspireMDAW2025",
            Email = "jct576@inlumine.ual.es"
        }
    });

    // Configurar JWT en Swagger
    options.AddSecurityDefinition("Bearer", new Microsoft.OpenApi.Models.OpenApiSecurityScheme
    {
        Description = "JWT Authorization header usando el esquema Bearer. Ejemplo: \"Authorization: Bearer {token}\"",
        Name = "Authorization",
        In = Microsoft.OpenApi.Models.ParameterLocation.Header,
        Type = Microsoft.OpenApi.Models.SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });

    options.AddSecurityRequirement(new Microsoft.OpenApi.Models.OpenApiSecurityRequirement
    {
        {
            new Microsoft.OpenApi.Models.OpenApiSecurityScheme
            {
                Reference = new Microsoft.OpenApi.Models.OpenApiReference
                {
                    Type = Microsoft.OpenApi.Models.ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

// Application Layer (MediatR, FluentValidation)
builder.Services.AddApplicationServices();

// Infrastructure Layer (DbContext, Identity, JWT, Repositories, MassTransit)
builder.Services.AddInfrastructureServices(builder.Configuration);

// CORS (opcional, para desarrollo)
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});

// Health Checks
builder.Services.AddHealthChecks()
    .AddDbContextCheck<AuthDbContext>("authdb");

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        options.SwaggerEndpoint("/swagger/v1/swagger.json", "Auth Service API v1");
        options.RoutePrefix = string.Empty; // Swagger UI en la raíz (http://localhost:5001)
    });
}

app.UseHttpsRedirection();

app.UseCors("AllowAll");

// Autenticación y Autorización
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

// Health Check endpoints
app.MapHealthChecks("/health");
app.MapHealthChecks("/alive");

app.Run();
