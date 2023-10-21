using System.ComponentModel.DataAnnotations;

namespace MyBookOrder.Models
{
    public class Book
    {
        [Key]
        [StringLength(5)]
        [RegularExpression(@"^BK[0-9][0-9][0-9]$")]
        public string BookId { get; set; }
        [MaxLength(50)]
        public string BookTitle { get; set; }
        [MaxLength (50)]
        public string BookType { get; set; }
        public int Price { get; set; }

        public ICollection<Order> Orders { get; set;}
    }
}
