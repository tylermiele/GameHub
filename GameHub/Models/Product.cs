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
        [DisplayFormat(DataFormatString  = "{0:c}")]
        public decimal? Price { get; set; }

        public string? Photo { get; set; }

        //FK for Category
        [Display(Name = "Category")]

        public int CategoryId { get; set; }

        //reference to parent model
        public Category? Category { get; set; }

        //reference to child model - 1 product can be in many cart items
        public List<CartItem>? CartItems { get; set; }

        //reference to 2nd child model - 1 product can be in many order details (bought many items)
        public List<OrderDetail>? OrderDetails { get; set; }

        [Display(Name = "Release Year")]
        public int ReleaseYear { get; set; }
    }
}
