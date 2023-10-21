namespace MyBookOrder.Models.Result
{
    public class GetOrderResult
    {
        public string OrderId { get; set; }
        public int Quantity { get; set; }
        public int TotalPrice { get; set; }
        public Book Book { get; set; }
    }
}
