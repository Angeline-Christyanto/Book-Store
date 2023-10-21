using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyBookOrder.Models
{
    public class Order
    {
        [Key]
        [StringLength(5)]
        [RegularExpression(@"^OR[0-9][0-9][0-9]$")]
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        // new added
        public int TotalPrice { get; set; }

        [ForeignKey("Book")]
        public string BookId { get; set; }
        public Book Book { get; set; }
    }
}
