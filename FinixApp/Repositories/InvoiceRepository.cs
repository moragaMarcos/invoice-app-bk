namespace backend.Repositories
{
    using backend.Data.ModelsEF;
    using backend.Interfaces;
    using Microsoft.EntityFrameworkCore;
    public class InvoiceRepository : IInvoiceRepository
    {
        private readonly AppDbContext _context;

        public InvoiceRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<TInvoice>> GetAllAsync()
        {
            return await _context.TInvoices
                .Include(i => i.TCustomer)
                .Include(i => i.TInvoiceDetail)
                .Include(i => i.TInvoiceCreditNote)
                .Include(i => i.TInvoicePayment)
                .ToListAsync();
        }

        public async Task<TInvoice?> GetByIdAsync(long invoiceNumber)
        {
            return await _context.TInvoices
                .Include(i => i.TCustomer)
                .Include(i => i.TInvoiceDetail)
                .Include(i => i.TInvoiceCreditNote)
                .Include(i => i.TInvoicePayment)
                .AsTracking()
                .FirstOrDefaultAsync(i => i.InvoiceNumber == invoiceNumber);
        }
        public async Task SaveChangesAsync() => await _context.SaveChangesAsync();
        public async Task<long> GetLastCreditNoteNumberAsync()
        {
            return await _context.TCreditNotes
                .OrderByDescending(n => n.CreditNoteNumber)
                .Select(n => n.CreditNoteNumber)
                .FirstOrDefaultAsync();
        }

    }

}

