namespace backend.Interfaces
{
    using Microsoft.AspNetCore.Mvc;
    public interface IExceptionHandlerService
    {
        Task<IActionResult> HandleAsync(Func<Task<IActionResult>> action, string context);
    }
}
