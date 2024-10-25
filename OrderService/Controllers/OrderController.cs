using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderDBContext;
using OrderModels;

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

        [HttpGet("GetOrders")]
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

        [HttpGet("TestOrder")]
        public  IActionResult TestOrder(int id)
        {
            List<string> items = new List<string>
        {
            "Item 1",
            "Item 2",
            "Item 3",
            "Item 4"
        };

            return Ok(items);
            //return Ok("Hello from Order API!");
        }

    }
}
