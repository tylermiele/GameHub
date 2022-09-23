using System.ComponentModel.DataAnnotations;

namespace GameHub.Models
{
    public class Product
    {
        public int ProductId { get; set; }

        [Required]
        [MinLength (2, ErrorMessage = "Must be at least 2 characters")]
        [MaxLength (100)]
        public string? Name { get; set; }

        [Range (0.01, 99999)]
        public decimal? Price { get; set; }
    }
}
