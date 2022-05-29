using System.ComponentModel.DataAnnotations;

namespace WebAPI.Dtos
{
    public class CityDto
    {
        public int Id { get; set; }
        [Required]
        [StringLength(50,MinimumLength=2)]
        [RegularExpression(".*[a-zA-Z]+.*",ErrorMessage ="Only numerics are not allowed")]
        public string Name { get; set; } = String.Empty;
        [Required]

        public string Country { get; set; } = String.Empty;

    }
}
