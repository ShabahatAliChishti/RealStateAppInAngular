using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class User:BaseEntity
    {
        [Required]
        public string Username { get; set; }


        //use byte[] to store binary password
        [Required]

        public byte[] Password { get; set; }
        //salt key

        public byte[] PasswordKey { get; set; }
    }
}
