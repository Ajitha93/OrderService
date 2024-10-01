using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public OrderController(RestaurantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Order>>> GetOrders()
        {
            var orders = await _context.Orders
               .Include(o => o.Customer)
               .Include(o => o.OrderProducts)
               .ThenInclude(op => op.Product)
               .Select(o => new OrderVM
               {
                   Id = o.Id,
                   CustomerName = o.Customer.Name,
                   Products = o.OrderProducts.Select(op => new ProductVM
                   {
                       Name = op.Product.Name,
                       Price = op.Product.Price,
                       Quantity = op.Quantity
                   }).ToList(),
                   OrderDate = o.OrderDate                  
               })
               .ToListAsync();

            return Ok(orders);
        }

        [Route("customer")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Customer>>> GetCustomers()
        {
            var customers= await _context.Customers
                .Select(c => new CustomerVM
                {
                    Id = c.Id,
                    Name=c.Name
                })
                .ToListAsync();
            return Ok(customers);
        }

        //[HttpGet("{id}")]
        //public async Task<ActionResult<Order>> GetOrder(int id)
        //{
        //    var order = await _context.Orders.Include(o => o.Product).Include(o => o.Customer).FirstOrDefaultAsync(o => o.Id == id);
        //    if (order == null) return NotFound();
        //    return order;
        //}

        //[HttpPost]
        //public async Task<ActionResult<Order>> PostOrder(Order order)
        //{
        //    _context.Orders.Add(order);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetOrder", new { id = order.Id }, order);
        //}
    }
}
