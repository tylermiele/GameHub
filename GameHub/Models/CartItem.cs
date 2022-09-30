using System.ComponentModel.DataAnnotations;

namespace GameHub.Models
{
    public class CartItem
    {
        public int CartItemId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public string? CustomerId { get; set; }

        [Required]
        public int ProductId { get; set; }

        //parent reference to Product - every cart item is for only 1 poduct
        public Product? Product { get; set; }
    }
}
