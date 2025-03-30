namespace backend.Data.Models.Response
{
    public class ProductResponse
    {
        public string Name { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }
        public decimal SubTotal { get; set; }

    }
}
