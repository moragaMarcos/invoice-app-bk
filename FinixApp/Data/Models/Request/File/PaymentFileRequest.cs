namespace backend.Data.Models.Request.File
{
    using System.Text.Json.Serialization;
    public class PaymentFileRequest
    {
        [JsonPropertyName("payment_method")]
        public string? PaymentMethod { get; set; }
        [JsonPropertyName("payment_date")]
        public DateTime? PaymentDate { get; set; }
    }
}
