namespace FileParser.Entities
{
    public class OrderDetail
    {
        public int Idx { get; set; }
        public string OrderIdx { get; set; }
        public int LineNumber { get; set; }
        public int Quantity { get; set; }
        public decimal CostEach { get; set; }
        public decimal TotalCost { get; set; }
        public string Description { get; set; }
        public OrderError OrderDetailOrderError { get; set; }
    }
}
