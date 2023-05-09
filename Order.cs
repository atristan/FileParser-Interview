namespace FileParser
{
    public class Order
    {
        public int Idx { get; set; }
        public int CustomerIdx { get; set; }
        public int AddressIdx { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }
        public string CustomerEmail { get; set; }
        public int OrderNumber { get; set; }
        public int TotalItems { get; set; }
        public decimal TotalCost { get; set; }
        public DateTime OrderDate { get; set; }
        public bool Paid { get; set; }
        public bool Shipped { get; set; }
        public bool Completed { get; set; }
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
        public bool Success { get; set; } = false;
        public bool Processed { get; set; } = false;
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
