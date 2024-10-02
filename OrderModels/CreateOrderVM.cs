using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderModels
{
    public class CreateOrderVM
    {
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }
        public List<OrderProductDto> Products { get; set; }
    }
    public class OrderProductDto
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
