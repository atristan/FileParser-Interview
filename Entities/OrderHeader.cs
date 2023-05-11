using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Entities
{
    public class OrderHeader
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
        public bool Success { get; set; }
        public bool Processed { get; set; }
        public OrderError OrderHeaderError { get; set; }
    }
}
