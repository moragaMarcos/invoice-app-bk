namespace backend.Data.ModelsEF
{
    using System.ComponentModel.DataAnnotations;

    public class TCreditNote
    {
        [Key]
        public long CreditNoteNumber { get; set; }
        public long InvoiceNumber { get; set; }
        public DateTime CreditNoteDate { get; set; }
        public decimal CreditNoteAmount { get; set; }
        public virtual TInvoice TInvoice { get; set; }
    }
}


/* Schema:
 
	{
		"credit_note_number": 5845,
		"credit_note_date": "2025-02-21",
		"credit_note_amount": 195454
	}

*/