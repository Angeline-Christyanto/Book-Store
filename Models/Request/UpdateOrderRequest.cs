using System.ComponentModel.DataAnnotations;

namespace MyBookOrder.Models.Request
{
    public class UpdateOrderRequest
    {
        [Required]
        public int Quantity { get; set; }
        [Required]
        public string BookId { get; set; }
    }
}
