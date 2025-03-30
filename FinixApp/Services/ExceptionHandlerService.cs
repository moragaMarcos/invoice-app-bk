namespace backend.Services
{
    using backend.Interfaces;
    using Microsoft.AspNetCore.Mvc;
    public class ExceptionHandlerService : IExceptionHandlerService
    {
        private readonly ILogger<ExceptionHandlerService> _logger;

        public ExceptionHandlerService(ILogger<ExceptionHandlerService> logger)
        {
            _logger = logger;
        }

        public async Task<IActionResult> HandleAsync(Func<Task<IActionResult>> action, string context)
        {
            try
            {
                return await action();
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Argumento inválido en {Context}", context);
                return new BadRequestObjectResult(new
                {
                    title = "Parámetros inválidos.",
                    detail = ex.Message,
                    type = ex.GetType().Name,
                    context,
                    code = StatusCodes.Status400BadRequest
                });
            }
            catch (KeyNotFoundException ex)
            {
                _logger.LogWarning(ex, "No se encontró el recurso en {Context}", context);
                return new NotFoundObjectResult(new
                {
                    title = "No se encontró el recurso solicitado.",
                    detail = ex.Message,
                    type = ex.GetType().Name,
                    context,
                    code = StatusCodes.Status404NotFound
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado en {Context}: {Message}", context, ex.Message);
                return new ObjectResult(new
                {
                    title = "Ha ocurrido un error inesperado.",
                    detail = ex.Message,
                    type = ex.GetType().Name,
                    context,
                    code = StatusCodes.Status500InternalServerError
                })
                {
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
        }

    }
}
