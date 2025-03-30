namespace backend.Data.ModelsEF
{
    using System.ComponentModel.DataAnnotations;

    public class TUser
    {
        public TUser()
        {
            UserId = Guid.NewGuid();
        }

        public Guid UserId { get; set; }

        [Required]
        public string Username { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required]
        public string PasswordHash { get; set; }

        public long RoleId { get; set; }
        public TRole Role { get; set; }
    }

}


/* Schema
  {
  	"UserId": long
  	"Username": string
    "Email": string
    "PasswordHash": hash
    "RoleId" long
  }
*/