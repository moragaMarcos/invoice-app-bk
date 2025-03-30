namespace backend.Data.Models.Request.File
{
    using System.Text.Json.Serialization;
    public class CustomerFileRequest
    {
        [JsonPropertyName("customer_run")]
        public string CustomerRun { get; set; }

        [JsonPropertyName("customer_name")]
        public string CustomerName { get; set; }

        [JsonPropertyName("customer_email")]
        public string CustomerEmail { get; set; }
    }
}
