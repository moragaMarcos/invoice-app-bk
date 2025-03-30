namespace backend.Data.Models.Request.File
{
    using System.Text.Json.Serialization;
    public class ProductFileRequest
    {
        [JsonPropertyName("product_name")]
        public string ProductName { get; set; }
        [JsonPropertyName("unit_price")]
        public decimal UnitPrice { get; set; }
        [JsonPropertyName("quantity")]
        public int Quantity { get; set; }
        [JsonPropertyName("subtotal")]
        public decimal SubTotal { get; set; }
    }
}
