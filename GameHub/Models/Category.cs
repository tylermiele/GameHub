using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;
using RequiredAttribute = System.ComponentModel.DataAnnotations.RequiredAttribute;

namespace GameHub.Models
{
    public class Category
    {
        public int CategoryId { get; set; }

        [Required]
        [MaxLength(50)]
        public string? Name { get; set; }

        //reference list of child Products. 1 to many relationship
        public List<Product>? Products { get; set; }
    }
}
