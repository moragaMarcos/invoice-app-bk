namespace backend.Data.Models.Response
{
    public class InvoiceResponse
    {
        public long InvoiceNumber { get; set; }
        public DateTime InvoiceDate { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime PaymentDueDate { get; set; }

        public int DaysToDue { get; set; }

        public string InvoiceStatus { get; set; }
        public string PaymentStatus { get; set; }

        public CustomerResponse Customer { get; set; }
        public List<ProductResponse> Products { get; set; }
        public List<CreditNoteResponse> CreditNotes { get; set; }
        public PaymentResponse? Payment { get; set; }
    }

}
