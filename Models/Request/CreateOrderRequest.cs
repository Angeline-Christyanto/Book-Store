using System.ComponentModel.DataAnnotations;

namespace MyBookOrder.Models.Request
{
    public class CreateOrderRequest
    {
        [Required]
        public string OrderId { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string BookId { get; set; }
    }
}
