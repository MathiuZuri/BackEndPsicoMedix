using System.Net;
using System.Text.Json;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Utils.Middlewares;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly IWebHostEnvironment _environment;

    public ExceptionMiddleware(RequestDelegate next, IWebHostEnvironment environment)
    {
        _next = next;
        _environment = environment;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (KeyNotFoundException ex)
        {
            await ResponderErrorAsync(
                context,
                HttpStatusCode.NotFound,
                ex.Message
            );
        }
        catch (InvalidOperationException ex)
        {
            await ResponderErrorAsync(
                context,
                HttpStatusCode.BadRequest,
                ex.Message
            );
        }
        catch (UnauthorizedAccessException ex)
        {
            await ResponderErrorAsync(
                context,
                HttpStatusCode.Unauthorized,
                ex.Message
            );
        }
        catch (Exception ex)
        {
            var mensaje = _environment.IsDevelopment()
                ? ex.Message
                : "Ocurrió un error interno en el servidor.";

            await ResponderErrorAsync(
                context,
                HttpStatusCode.InternalServerError,
                mensaje
            );
        }
    }

    private static async Task ResponderErrorAsync(
        HttpContext context,
        HttpStatusCode statusCode,
        string mensaje)
    {
        if (context.Response.HasStarted)
            return;

        context.Response.Clear();
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)statusCode;

        var respuesta = ApiResponse<object>.Error(
            mensaje,
            context.Response.StatusCode
        );

        var json = JsonSerializer.Serialize(respuesta, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        });

        await context.Response.WriteAsync(json);
    }
}