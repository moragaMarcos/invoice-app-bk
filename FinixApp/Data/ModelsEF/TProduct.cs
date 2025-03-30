namespace backend.Data.ModelsEF
{
    using System.ComponentModel.DataAnnotations;

    public class TProduct
    {
        [Key]
        public long ProductNumber { get; set; }
        public long InvoiceNumber { get; set; }
        [MaxLength(100)]
        public string ProductName { get; set; }
        public decimal UnitPrice { get; set; }
        public int Quantity { get; set; }

        public decimal SubTotal { get; set; }

        public virtual TInvoice TInvoice { get; set; }
    }
}

/* Schema
  {
  	"product_name": "Lettuce - Baby Salad Greens",
  	"unit_price": 3193,
  	"quantity": 83,
  	"subtotal": 265019
  }
*/