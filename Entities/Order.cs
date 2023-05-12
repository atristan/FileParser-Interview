using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FileParser.Entities
{
    public class Order
    {
        public int Id { get; set; }
        public OrderHeader Header { get; set; }
        public OrderAddress Address { get; set; }
        public OrderDetail Detail { get; set; }
        public List<OrderError> Errors { get; set; }
    }
}
