using System;
using System.Collections.Generic;


namespace OrderModels {

    public partial class Order
    {
        public Guid Id { get; set; }
        public int CustomerId { get; set; }
        public DateTime OrderDate { get; set; }

        // Navigation properties
        public Customer Customer { get; set; }
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }
}
