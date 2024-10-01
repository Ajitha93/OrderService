namespace OrderService.Models
{
    public class OrderVM
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public List<ProductVM> Products { get; set; }
        public DateTime? OrderDate { get; set; }
       
    }
}
