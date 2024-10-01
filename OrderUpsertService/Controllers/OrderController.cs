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
    }
}
