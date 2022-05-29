using System.ComponentModel.DataAnnotations;

namespace WebAPI.Models
{
    public class City : BaseEntity
    {
        public int Id { get; set; }
        public string Name { get; set; } = String.Empty;
        [Required]
        public string Country { get; set; } = String.Empty;
        //public DateTime LastUpdatedOn { get; set; }

        //public int LastUpdatedBy { get; set; }
    }

}
