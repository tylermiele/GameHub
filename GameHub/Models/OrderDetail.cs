using System.ComponentModel.DataAnnotations;

namespace GameHub.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public double Price { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int ProductId { get; set; }

        //parent reference - 1 order many order details
        public Order? Order { get; set; }

        // 2nd parent reference - 1 product in many order details
        public Product? Product { get; set; }
    }
}
