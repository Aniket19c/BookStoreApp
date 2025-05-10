namespace RepositoryLayer.DTO
{
    public class OrderResponse
    {
        public int OrderId { get; set; }
        public string BookName { get; set; }
        public int Quantity { get; set; }
        public decimal TotalAmount { get; set; }
        public string Status { get; set; }
        public DateTime OrderDate { get; set; }
        public string ShippingAddress { get; set; }
    }
}
