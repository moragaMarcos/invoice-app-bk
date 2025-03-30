namespace backend.Services.Imports
{
    using backend.Data.Enums;
    using backend.Data.ModelsEF;
    using System.Text.Json;
    using Microsoft.EntityFrameworkCore;
    using backend.Utils;
    using backend.Interfaces;
    using backend.Data.Models.Request.File;

    public class InvoiceImport : IInvoiceImport
    {
        private readonly AppDbContext _context;
        private readonly ILogger<InvoiceImport> _logger;

        public InvoiceImport(AppDbContext context, ILogger<InvoiceImport> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task ImportAsync()
        {
            var filePath = Path.Combine(Directory.GetCurrentDirectory(), "bd_exam.json");
            if (!File.Exists(filePath))
            {
                _logger.LogWarning("No se encontró el archivo bd_exam.json");
                return;
            }

            var json = await File.ReadAllTextAsync(filePath);
            var file = JsonSerializer.Deserialize<InvoiceWrapperFileRequest>(json, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
            var invoices = file?.Invoices ?? new List<InvoiceFileRequest>();

            if (invoices == null)
            {
                _logger.LogWarning("No se encontraron facturas en el archivo.");
                return;
            }

            int total = invoices.Count;
            int inserted = 0;
            int skipped = 0;

            foreach (var dto in invoices)
            {
                bool exists = await _context.TInvoices.AnyAsync(i => i.InvoiceNumber == dto.InvoiceNumber);
                bool invalidAmount = dto.TotalAmount != dto.InvoiceDetail.Sum(p => p.SubTotal);

                if (exists || invalidAmount)
                {
                    skipped++;
                    _logger.LogWarning("Factura {InvoiceNumber} omitida ({Reason})",
                        dto.InvoiceNumber,
                        exists ? "duplicada" : "monto inválido");
                    continue;
                }

                var customer = await _context.TCustomers.FindAsync(dto.Customer.CustomerRun) ??
                    new TCustomer
                    {
                        Run = dto.Customer.CustomerRun,
                        Name = dto.Customer.CustomerName,
                        Email = dto.Customer.CustomerEmail
                    };

                var invoice = new TInvoice
                {
                    InvoiceNumber = dto.InvoiceNumber,
                    InvoiceDate = dto.InvoiceDate,
                    TotalAmount = dto.TotalAmount,
                    PaymentDueDate = dto.PaymentDueDate,
                    TCustomer = customer,
                    TInvoiceDetail = dto.InvoiceDetail.Select(p => new TProduct
                    {
                        ProductName = p.ProductName,
                        UnitPrice = p.UnitPrice,
                        Quantity = p.Quantity,
                        SubTotal = p.SubTotal
                    }).ToList(),
                    TInvoiceCreditNote = dto.InvoiceCreditNote.Select(cn => new TCreditNote
                    {
                        CreditNoteNumber = cn.CreditNoteNumber,
                        CreditNoteAmount = cn.CreditNoteAmount,
                        CreditNoteDate = cn.CreditNoteDate
                    }).ToList(),
                    TInvoicePayment = dto.InvoicePayment == null ? null : new TPayment
                    {
                        PaymentMethod = dto.InvoicePayment.PaymentMethod,
                        PaymentDate = dto.InvoicePayment.PaymentDate
                    }
                };

                var creditTotal = invoice.TInvoiceCreditNote.Sum(cn => cn.CreditNoteAmount);
                invoice.InvoiceStatus = creditTotal switch
                {
                    0 => InvoiceStatusEnum.Issued,
                    var sum when sum == invoice.TotalAmount => InvoiceStatusEnum.Cancelled,
                    var sum when sum < invoice.TotalAmount => InvoiceStatusEnum.Partial,
                    _ => InvoiceStatusEnum.Issued
                };
                var hasPayment = dto.InvoicePayment != null &&
                     !string.IsNullOrWhiteSpace(dto.InvoicePayment.PaymentMethod) &&
                     dto.InvoicePayment.PaymentDate != null;

                invoice.PaymentStatus = hasPayment
                    ? PaymentStatusEnum.Paid
                    : DateUtils.GetCLDateNow() > dto.PaymentDueDate
                        ? PaymentStatusEnum.Overdue
                        : PaymentStatusEnum.Pending;

                _context.TInvoices.Add(invoice);
                inserted++;
            }

            await _context.SaveChangesAsync();

            _logger.LogInformation("Resumen de importación:");
            _logger.LogInformation("Total de facturas procesadas: {Total}", total);
            _logger.LogInformation("Facturas insertadas: {Inserted}", inserted);
            _logger.LogInformation("Facturas omitidas: {Skipped}", skipped);
        }
    }
}
