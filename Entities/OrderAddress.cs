namespace FileParser.Entities
{
    public class OrderAddress
    {
        public int Idx { get; set; }
        public int CustomerIdx { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Zip { get; set; }
        public bool Processed { get; set; }
        public Exception Error { get; set; }
    }
}
