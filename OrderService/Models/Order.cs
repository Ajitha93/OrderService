using System;
using System.Collections.Generic;

namespace OrderService.Models;

public partial class Order
{
    public int Id { get; set; }

    public int CustomerId { get; set; }    

    public DateTime? OrderDate { get; set; }
    
    public virtual Customer Customer { get; set; } = null!;    

    public ICollection<OrderProduct> OrderProducts { get; set; }
}
