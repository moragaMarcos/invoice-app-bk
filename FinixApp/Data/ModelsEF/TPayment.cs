namespace backend.Data.ModelsEF
{
    using System.ComponentModel.DataAnnotations;

    public class TPayment
    {
        [Key]
        public long PaymentNumber { get; set; }
        public long InvoiceNumber { get; set; }

        public string? PaymentMethod { get; set; }
        public DateTime? PaymentDate { get; set; }

        public TInvoice TInvoice { get; set; }
    }
}


/* Schema
 
 {
	"payment_method": null,
	"payment_date": null
 } 

*/