using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;

namespace Backend_App_Tareas_Hogar.Utilities.Helpers.Validators
{
    public class DatabaseExceptionMiddleware
    {
        private readonly RequestDelegate _next;

        // El constructor recibe el siguiente paso del pipeline
        public DatabaseExceptionMiddleware(RequestDelegate next)
        {
            _next = next; // Guardamos el delegado para que el middleware pueda continuar el flujo
        }

        // Este método se ejecuta para cada request
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                // Intentamos ejecutar la siguiente parte del pipeline (controlador, handler, etc.)
                await _next(context);
            }
            catch (Exception ex) // Capturamos cualquier excepción
            {
                // Delegamos a un método que procesará la excepción
                await HandleExceptionAsync(context, ex);
            }
        }

        // Método que define la respuesta dependiendo del tipo de excepción
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            // Por defecto devolvemos 500
            var statusCode = HttpStatusCode.InternalServerError;
            var errorMessage = "Ha ocurrido un error inesperado";

            // Detectamos si es un error de Postgres
            if (exception is NpgsqlException npgEx)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = "Error en la base de datos (Postgres).";

                // TODO: aquí puedes inspeccionar npgEx.SqlState para obtener códigos como 23505 (unique violation)
            }
            else if (exception is DbUpdateException dbEx)
            {
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = "No se pudo guardar cambios en la base de datos.";

                // TODO: puedes inspeccionar dbEx.InnerException para más detalles
            }
            else if (exception is InvalidOperationException invalidOpEx)
            {
                // Muchas veces EF Core lanza esta excepción si la consulta falla
                statusCode = HttpStatusCode.BadRequest;
                errorMessage = "Operación inválida al interactuar con los datos.";

                // TODO: agregar manejo más especializado si deseas
            }
            else
            {
                // Cualquier otra excepción
                statusCode = HttpStatusCode.InternalServerError;
            }

            // Construimos la respuesta JSON
            var result = new
            {
                status = (int)statusCode,
                errors = errorMessage,
                detail = exception.Message // TODO: puedes ocultar esto en producción
            };

            // Configuramos el tipo de contenido
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            // Serializamos el JSON y lo enviamos
            await context.Response.WriteAsJsonAsync(result);
        }
    }

    // Método de extensión para registrar el middleware
    public static class DatabaseExceptionMiddlewareExtensions
    {
        public static IApplicationBuilder UseDatabaseExceptionMiddleware(this IApplicationBuilder app)
        {
            return app.UseMiddleware<DatabaseExceptionMiddleware>();
        }
    }
}
