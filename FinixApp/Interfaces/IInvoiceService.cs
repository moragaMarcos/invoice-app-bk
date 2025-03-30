namespace backend.Interfaces
{
    using backend.Data.Enums;
    using backend.Data.Models.Request;
    using backend.Data.Models.Response;

    public interface IInvoiceService
    {
        Task<List<InvoiceResponse>> GetAllAsync();
        Task<InvoiceResponse?> GetByIdAsync(long invoiceNumber);
        Task<List<InvoiceResponse>> GetFilteredAsync(InvoiceStatusEnum? status, PaymentStatusEnum? paymentStatus);
        Task<CreditNoteResponse> AddCreditNoteAsync(long invoiceNumber, CreditNoteRequest request);
        Task<List<CreditNoteResponse>> GetCreditNotesAsync(long invoiceNumber);

    }
}
