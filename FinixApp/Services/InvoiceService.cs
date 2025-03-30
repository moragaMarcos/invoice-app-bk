namespace backend.Services
{
    using backend.Data.Enums;
    using backend.Data.Models.Request;
    using backend.Data.Models.Response;
    using backend.Data.ModelsEF;
    using backend.Interfaces;
    using backend.Utils;
    public class InvoiceService : IInvoiceService
    {
        private readonly IInvoiceRepository _repository;
        private readonly AppDbContext _context;
        public InvoiceService(IInvoiceRepository repository, AppDbContext context)
        {
            _repository = repository;
            _context = context;
        }

        public async Task<List<InvoiceResponse>> GetAllAsync()
        {
            var invoices = await _repository.GetAllAsync();

            return invoices.Select(ToResponse).ToList();
        }

        public async Task<InvoiceResponse?> GetByIdAsync(long invoiceNumber)
        {
            var invoice = await _repository.GetByIdAsync(invoiceNumber);
            return invoice == null ? null : ToResponse(invoice);
        }

        private InvoiceResponse ToResponse(TInvoice invoice)
        {
            return new InvoiceResponse
            {
                InvoiceNumber = invoice.InvoiceNumber,
                InvoiceDate = invoice.InvoiceDate,
                TotalAmount = invoice.TotalAmount,
                PaymentDueDate = invoice.PaymentDueDate,
                DaysToDue = DateUtils.CalculateDaysToDueCL(invoice.PaymentDueDate),
                InvoiceStatus = invoice.InvoiceStatus.ToString(),
                PaymentStatus = invoice.PaymentStatus.ToString(),

                Customer = new CustomerResponse
                {
                    Run = invoice.TCustomer.Run,
                    Name = invoice.TCustomer.Name,
                    Email = invoice.TCustomer.Email
                },

                Products = invoice.TInvoiceDetail.Select(p => new ProductResponse
                {
                    Name = p.ProductName,
                    UnitPrice = p.UnitPrice,
                    Quantity = p.Quantity,
                    SubTotal = p.SubTotal
                }).ToList(),

                CreditNotes = invoice.TInvoiceCreditNote.Select(cn => new CreditNoteResponse
                {
                    CreditNoteNumber = cn.CreditNoteNumber,
                    CreditNoteDate = cn.CreditNoteDate,
                    CreditNoteAmount = cn.CreditNoteAmount
                }).ToList(),

                Payment = invoice.TInvoicePayment == null ? null : new PaymentResponse
                {
                    PaymentMethod = invoice.TInvoicePayment.PaymentMethod,
                    PaymentDate = invoice.TInvoicePayment.PaymentDate
                }
            };
        }

        public async Task<List<InvoiceResponse>> GetFilteredAsync(InvoiceStatusEnum? status, PaymentStatusEnum? paymentStatus)
        {
            var invoices = await _repository.GetAllAsync();

            if (status != null)
                invoices = invoices.Where(i => i.InvoiceStatus == status).ToList();

            if (paymentStatus != null)
                invoices = invoices.Where(i => i.PaymentStatus == paymentStatus).ToList();

            return invoices.Select(ToResponse).ToList();
        }
        public async Task<CreditNoteResponse> AddCreditNoteAsync(long invoiceNumber, CreditNoteRequest request)
        {
            var invoice = await _repository.GetByIdAsync(invoiceNumber);
            if (invoice == null)
                throw new KeyNotFoundException($"La factura #{invoiceNumber} no existe.");

            var totalNotas = invoice.TInvoiceCreditNote.Sum(n => n.CreditNoteAmount);
            var saldoDisponible = invoice.TotalAmount - totalNotas;

            if (request.CreditNoteAmount <= 0)
                throw new ArgumentException("El monto de la nota de crédito debe ser mayor que cero.");

            if (request.CreditNoteAmount > saldoDisponible)
                throw new ArgumentException($"El monto excede el saldo restante de la factura. Máximo permitido: {saldoDisponible}.");

            var nuevaNota = new TCreditNote
            {
                CreditNoteAmount = request.CreditNoteAmount,
                CreditNoteDate = DateUtils.GetCLDateNow(),
                InvoiceNumber = invoice.InvoiceNumber,
            };

            invoice.TInvoiceCreditNote.Add(nuevaNota);

            var sumaNotas = invoice.TInvoiceCreditNote.Sum(n => n.CreditNoteAmount);
            if (sumaNotas == 0)
                invoice.InvoiceStatus = InvoiceStatusEnum.Issued;
            else if (sumaNotas >= invoice.TotalAmount)
                invoice.InvoiceStatus = InvoiceStatusEnum.Cancelled;
            else
                invoice.InvoiceStatus = InvoiceStatusEnum.Partial;

            await _repository.SaveChangesAsync();

            return new CreditNoteResponse
            {
                CreditNoteNumber = nuevaNota.CreditNoteNumber,
                CreditNoteAmount = nuevaNota.CreditNoteAmount,
                CreditNoteDate = nuevaNota.CreditNoteDate
            };
        }
        public async Task<List<CreditNoteResponse>> GetCreditNotesAsync(long invoiceNumber)
        {
            var invoice = await _repository.GetByIdAsync(invoiceNumber);
            if (invoice == null)
                throw new KeyNotFoundException($"La factura #{invoiceNumber} no existe.");

            return invoice.TInvoiceCreditNote
                .Select(n => new CreditNoteResponse
                {
                    CreditNoteNumber = n.CreditNoteNumber,
                    CreditNoteAmount = n.CreditNoteAmount,
                    CreditNoteDate = n.CreditNoteDate
                }).ToList();
        }


    }

}
