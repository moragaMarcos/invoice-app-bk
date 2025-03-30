namespace backend.Data.Models.Request.File
{
    using System.Text.Json.Serialization;
    public class CreditNoteFileRequest
    {
        [JsonPropertyName("credit_note_number")]
        public int CreditNoteNumber { get; set; }
        [JsonPropertyName("credit_note_date")]
        public DateTime CreditNoteDate { get; set; }
        [JsonPropertyName("credit_note_amount")]
        public decimal CreditNoteAmount { get; set; }
    }
}
