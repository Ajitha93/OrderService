﻿namespace OrderModels
{
    public class OrderVM
    {
        public Guid Id { get; set; }
        public string CustomerName { get; set; }
        public List<ProductVM> Products { get; set; }
        public DateTime? OrderDate { get; set; }
       
    }
}
