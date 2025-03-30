namespace backend.Data.ModelsEF
{
    using System.ComponentModel.DataAnnotations;
    using backend.Data.Enums;

    public class TInvoice
    {
        public TInvoice()
        {
            TInvoiceDetail = new HashSet<TProduct>();
            TInvoiceCreditNote = new HashSet<TCreditNote>();
        }
        [Key]
        public long InvoiceNumber { get; set; }

        [MaxLength(10)]
        public string CustomerRun { get; set; }

        public DateTime InvoiceDate { get; set; }
        public InvoiceStatusEnum InvoiceStatus { get; set; } = InvoiceStatusEnum.Issued;
        public decimal TotalAmount { get; set; }

        public DateTime PaymentDueDate { get; set; }
        public PaymentStatusEnum PaymentStatus { get; set; } = PaymentStatusEnum.Pending;

        public virtual ICollection<TProduct> TInvoiceDetail { get; set; }
        public virtual ICollection<TCreditNote> TInvoiceCreditNote { get; set; }

        public virtual TPayment? TInvoicePayment { get; set; }
        public virtual TCustomer TCustomer { get; set; }
    }

}

/* Schema:
 
	{
		"invoice_number": 1,
		"invoice_date": "2025-01-14",
		"invoice_status": "issued",
		"total_amount": 265019,
		"days_to_due": 75,
		"payment_due_date": "2025-03-30",
		"payment_status": "Pending",
		"invoice_detail": [],
		"invoice_payment": {},
		"invoice_credit_note": [],
		"customer": {}
	}

*/