using System;
using System.Collections.Generic;

namespace OrderModels
{
    public partial class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }

        // Navigation property
        public ICollection<OrderProduct> OrderProducts { get; set; }
    }

}

