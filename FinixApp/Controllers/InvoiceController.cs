using backend.Data.Enums;
using backend.Data.Models.Request;
using backend.Data.Models.Response;
using backend.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class InvoiceController : ControllerBase
{
    private readonly IInvoiceService _invoiceService;
    private readonly IExceptionHandlerService _exceptionHandler;

    public InvoiceController(IInvoiceService invoiceService, IExceptionHandlerService exceptionHandler)
    {
        _invoiceService = invoiceService;
        _exceptionHandler = exceptionHandler;
    }

    /// <summary>
    /// Obtiene todas las facturas del sistema.
    /// </summary>
    /// <returns>Listado completo de facturas.</returns>
    /// <response code="200">Facturas obtenidas correctamente</response>
    /// <response code="500">Error inesperado del servidor</response>
    [HttpGet]
    [ProducesResponseType(typeof(List<InvoiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll()
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            var invoices = await _invoiceService.GetAllAsync();
            return Ok(invoices);
        }, context: "GET /api/invoice");
    }

    /// <summary>
    /// Obtiene una factura específica por id.
    /// </summary>
    /// <param name="invoiceNumber">Identificador de la factura</param>
    /// <returns>Factura encontrada</returns>
    /// <response code="200">Factura encontrada correctamente</response>
    /// <response code="404">No se encontró la factura</response>
    /// <response code="500">Error inesperado del servidor</response>
    [HttpGet("{invoiceNumber}")]
    [ProducesResponseType(typeof(InvoiceResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(long invoiceNumber)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            if (invoiceNumber <= 0)
                throw new ArgumentException("El número de factura debe ser mayor que cero.");

            var invoice = await _invoiceService.GetByIdAsync(invoiceNumber);
            return invoice == null
                ? NotFound(new { message = $"No se encontró la factura #{invoiceNumber}." })
                : Ok(invoice);
        }, context: $"GET /api/invoice/{invoiceNumber}");
    }

    /// <summary>
    /// Busca facturas filtrando por estado de factura y/o estado de pago.
    /// </summary>
    /// <param name="invoiceStatus">Estado de factura: Issued, Cancelled, Partial</param>
    /// <param name="paymentStatus">Estado de pago: Pending, Overdue, Paid</param>
    /// <response code="200">Facturas filtradas correctamente</response>
    /// <response code="400">Parámetros inválidos</response>
    /// <response code="500">Error inesperado del servidor</response>
    [HttpGet("search")]
    [ProducesResponseType(typeof(List<InvoiceResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]

    public async Task<IActionResult> Search([FromQuery] string? invoiceStatus, [FromQuery] string? paymentStatus)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            InvoiceStatusEnum? invoiceStatusEnum = null;
            PaymentStatusEnum? paymentStatusEnum = null;

            if (!string.IsNullOrWhiteSpace(invoiceStatus))
            {
                if (!Enum.TryParse<InvoiceStatusEnum>(invoiceStatus, ignoreCase: true, out var parsedStatus))
                    throw new ArgumentException("El estado de factura no es válido. Valores posibles: Issued, Cancelled, Partial.");

                invoiceStatusEnum = parsedStatus;
            }
            if (!string.IsNullOrWhiteSpace(paymentStatus))
            {
                if (!Enum.TryParse<PaymentStatusEnum>(paymentStatus, ignoreCase: true, out var parsedPaymentStatus))
                    throw new ArgumentException("El estado de pago no es válido. Valores posibles: Pending, Overdue, Paid.");

                paymentStatusEnum = parsedPaymentStatus;
            }

            var invoices = await _invoiceService.GetFilteredAsync(invoiceStatusEnum, paymentStatusEnum);
            return Ok(invoices);
        }, context: "GET /api/invoice/search");
    }

    /// <summary>
    /// Agrega una nota de crédito a una factura existente.
    /// </summary>
    /// <param name="invoiceNumber">Número de la factura</param>
    /// <param name="request">Detalle de la nota de crédito</param>
    /// <response code="201">Nota de crédito creada correctamente</response>
    /// <response code="400">Error de validación</response>
    /// <response code="404">Factura no encontrada</response>
    /// <response code="500">Error inesperado</response>
    [HttpPost("{invoiceNumber}/credit-note")]
    [Authorize(Roles = "admin")]
    [ProducesResponseType(typeof(CreditNoteResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> AddCreditNote(long invoiceNumber, [FromBody] CreditNoteRequest request)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            var note = await _invoiceService.AddCreditNoteAsync(invoiceNumber, request);
            return CreatedAtAction(nameof(GetById), new { invoiceNumber }, note);
        }, context: $"POST /api/invoice/{invoiceNumber}/credit-note");
    }

    /// <summary>
    /// Obtiene todas las notas de crédito asociadas a una factura.
    /// </summary>
    /// <param name="invoiceNumber">Número de factura</param>
    /// <returns>Listado de notas de crédito</returns>
    /// <response code="200">Notas de crédito encontradas</response>
    /// <response code="404">Factura no existe</response>
    /// <response code="500">Error inesperado</response>
    [HttpGet("{invoiceNumber}/credit-notes")]
    [ProducesResponseType(typeof(List<CreditNoteResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetCreditNotes(long invoiceNumber)
    {
        return await _exceptionHandler.HandleAsync(async () =>
        {
            var notes = await _invoiceService.GetCreditNotesAsync(invoiceNumber);
            return Ok(notes);
        }, context: $"GET /api/invoice/{invoiceNumber}/credit-notes");
    }


}
