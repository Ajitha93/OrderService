using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OrderDBContext;
using OrderModels;

namespace OrderUpsertService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : Controller
    {
        private readonly RestaurantContext _context;

        public OrderController(RestaurantContext context)
        {
            _context = context;
        }

        [Route("customer")]
        [HttpGet]
        public async Task<ActionResult> GetCustomers()
        {
            var customers = await _context.Customers
                .Select(c => new CustomerVM
                {
                    Id = c.Id,
                    Name = c.Name
                })
                .ToListAsync();
            return Ok(customers);
        }

        [Route("product")]
        [HttpGet]
        public async Task<ActionResult> GetProducts()
        {
            var products = await _context.Products
                .Select(p => new ProductVM
                {
                    Id = p.Id,
                    Name = p.Name,
                    Price=p.Price
                })
                .ToListAsync();
            return Ok(products);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderVM request)
        {
            try
            {
                if (request == null || request.Products == null || !request.Products.Any())
                {
                    return BadRequest("Invalid order data.");
                }

                // Create a new Order
                var order = new Order
                {
                    Id=Guid.NewGuid(),
                    CustomerId = request.CustomerId,
                    OrderDate = request.OrderDate,
                    OrderProducts = new List<OrderProduct>()
                };

                // Populate OrderProducts
                foreach (var product in request.Products)
                {
                    var orderProduct = new OrderProduct
                    {
                        OrderId=order.Id,
                        ProductId = product.ProductId,
                        Quantity = product.Quantity
                    };
                    order.OrderProducts.Add(orderProduct);
                }

                // Add and save the order
                _context.Orders.Add(order);
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(CreateOrder), new { id = order.Id }, order);
            }
            catch(Exception ex)
            {
                return BadRequest("Error occured while saving data.");
            }
            
        }
    }
}
