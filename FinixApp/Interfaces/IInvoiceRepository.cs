namespace backend.Interfaces
{
    using backend.Data.ModelsEF;

    public interface IInvoiceRepository
    {
        Task<List<TInvoice>> GetAllAsync();
        Task<TInvoice?> GetByIdAsync(long invoiceNumber);
        Task SaveChangesAsync();
        Task<long> GetLastCreditNoteNumberAsync();

    }

}

