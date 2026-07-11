using Microsoft.AspNetCore.Mvc;
using psicomedixMonolito.Models;

namespace psicomedixMonolito.Utils.Configurations;

public static class ValidationResponseConfig
{
    public static void ConfigurarRespuestasDeValidacion(ApiBehaviorOptions options)
    {
        options.InvalidModelStateResponseFactory = context =>
        {
            var errores = context.ModelState
                .Where(x => x.Value?.Errors.Count > 0)
                .SelectMany(x => x.Value!.Errors)
                .Select(x => string.IsNullOrWhiteSpace(x.ErrorMessage)
                    ? "Error de validación en la solicitud."
                    : x.ErrorMessage)
                .Distinct()
                .ToList();

            var respuesta = ApiResponse<object>.Error(
                "La solicitud contiene errores de validación.",
                StatusCodes.Status400BadRequest,
                errores
            );

            return new BadRequestObjectResult(respuesta);
        };
    }
}