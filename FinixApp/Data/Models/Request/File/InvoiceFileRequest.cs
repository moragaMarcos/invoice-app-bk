namespace backend.Data.Models.Request.File
{
    using System.Text.Json.Serialization;

    public class InvoiceFileRequest
    {
        public InvoiceFileRequest()
        {
            InvoiceDetail = [];
            InvoiceCreditNote = [];
        }
        [JsonPropertyName("invoice_number")]
        public long InvoiceNumber { get; set; }
        [JsonPropertyName("invoice_date")]
        public DateTime InvoiceDate { get; set; }
        [JsonPropertyName("total_amount")]
        public decimal TotalAmount { get; set; }

        [JsonPropertyName("payment_due_date")]
        public DateTime PaymentDueDate { get; set; }
        [JsonPropertyName("invoice_payment")]
        public PaymentFileRequest? InvoicePayment { get; set; }
        [JsonPropertyName("invoice_detail")]
        public List<ProductFileRequest> InvoiceDetail { get; set; }
        [JsonPropertyName("invoice_credit_note")]
        public List<CreditNoteFileRequest> InvoiceCreditNote { get; set; }
        [JsonPropertyName("customer")]
        public CustomerFileRequest Customer { get; set; }
    }
}
