namespace backend.Data.ModelsEF
{
    using System.ComponentModel.DataAnnotations;

    public class TCustomer
    {
        public TCustomer()
        {
            TInvoices = new HashSet<TInvoice>();
        }
        [Key]
        [MaxLength(10)]
        public string Run { get; set; }

        [MaxLength(50)]
        public string Name { get; set; }
        [MaxLength(200)]
        public string Email { get; set; }

        public ICollection<TInvoice> TInvoices { get; set; }
    }
}


/* Schema
  
 {
	"customer_run": "13075795-2",
	"customer_name": "Juanita Hugh",
	"customer_email": "jhugh0@xinhuanet.com"
 }

*/