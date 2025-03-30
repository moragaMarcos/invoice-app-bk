namespace backend.Data.ModelsEF
{
    using System.ComponentModel.DataAnnotations;

    public class TRole
    {
        public TRole()
        {
            Users = new HashSet<TUser>();
        }
        public long RoleId { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<TUser> Users { get; set; }
    }

}

/* Schema
  {
  	"RoleId": long
  	"Name": string,
  }
*/