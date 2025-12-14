
using Backend_App_Tareas_Hogar.Application.Users.Register;
using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Backend_App_Tareas_Hogar.Infraestructure.Interfaces;
using Backend_App_Tareas_Hogar.Infraestructure.Services;
using Backend_App_Tareas_Hogar.Utilities.Helpers;
using Backend_App_Tareas_Hogar.Utilities.Helpers.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// ------------------ Servicios ------------------
builder.Services.AddTransient<DbContext, ApplicationDbContext>();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// FluentValidation (registro correcto)
builder.Services.AddValidatorsFromAssemblyContaining<RegisterValidator>();

// MediatR
builder.Services.AddMediatR(e =>
    e.RegisterServicesFromAssemblyContaining<RegisterCommand>());

// Agregar el ValidationBehavior al pipeline de MediatR
builder.Services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

// DATABASE Connection
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);

AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

// ------------------------------------------------------------------
// JWT CONFIGURATION

var key = Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]);

builder.Services
    .AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
    })
    .AddJwtBearer(options =>
    {
        options.RequireHttpsMetadata = false;
        options.SaveToken = true;

        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,

            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(key)
        };
    });

// Servicio JWT
builder.Services.AddSingleton<IJwtService, JwtService>();

// ------------------------------------------------------------------

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(Program).Assembly);
});


//------------------------------
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Configuración de manejo de excepciones global
// Esto captura las excepciones no manejadas y devuelve una respuesta JSON
app.UseExceptionHandler(appBuilder =>
{
    appBuilder.Run(async context =>
    {
        context.Response.ContentType = "application/json";
        var exception = context.Features.Get<IExceptionHandlerFeature>()?.Error;

        context.Response.StatusCode = exception switch
        {
            UnauthorizedAccessException => StatusCodes.Status403Forbidden,
            BadHttpRequestException => StatusCodes.Status400BadRequest,
            KeyNotFoundException => StatusCodes.Status404NotFound,
            ValidationException => StatusCodes.Status400BadRequest,
            NpgsqlException => StatusCodes.Status400BadRequest,
            DbUpdateException => StatusCodes.Status400BadRequest,
            InvalidOperationException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        object response;

        if (exception is ValidationException validationEx)
        {
            // Formato personalizado para errores de FluentValidation
            response = new
            {
                status = 400,
                message = "Error de validación",
                errors = validationEx.Errors.Select(e => e.ErrorMessage).ToList()
            };
        }
        else if(exception is NpgsqlException)
        {
            // Manejo específico para errores de Postgres
            response = new
            {
                status = context.Response.StatusCode,
                message = "Error en la base de datos (Postgres).",
                detail = exception.Message
            };
        }
        else if (exception is DbUpdateException)
        {
            // Manejo específico para errores de actualización de la base de datos
            response = new
            {
                status = context.Response.StatusCode,
                message = "No se pudo guardar cambios en la base de datos.",
                detail = exception.Message
            };
        }
        else if (exception is InvalidOperationException)
        {
            // Manejo específico para InvalidOperationException
            response = new
            {
                status = context.Response.StatusCode,
                message = "Operación inválida al interactuar con los datos.",
                detail = exception.Message
            };
        }
        else
        {
            // Para cualquier otro tipo de error
            response = new
            {
                status = context.Response.StatusCode,
                message = exception?.Message ?? "Ha ocurrido un error inesperado."
            };
        }

        await context.Response.WriteAsJsonAsync(response);
    });
});


app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
