
using Backend_App_Tareas_Hogar.Application.Users.Register;
using Backend_App_Tareas_Hogar.Infraestructure.Data;
using Backend_App_Tareas_Hogar.Utilities.Helpers;
using Backend_App_Tareas_Hogar.Utilities.Helpers.Validators;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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

var jwtKey = builder.Configuration["Jwt:Key"];
var jwtIssuer = builder.Configuration["Jwt:Issuer"];

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = false,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
        ClockSkew = TimeSpan.Zero
    };
});


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

app.UseDatabaseExceptionMiddleware();

app.UseAuthorization();

app.MapControllers();

app.Run();
