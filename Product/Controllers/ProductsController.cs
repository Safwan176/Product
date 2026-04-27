
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.Data;
using Products.Models;

namespace Product.Controllers
{

    [Route("api/[controller]")]  // Route: /api/products
    [ApiController]

    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        // Inject the database context via constructor
        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<IEnumerable<Products.Models.Products>>> GetAll()
        {
            // EF translates this to: SELECT * FROM Products
            return await _context.Products.ToListAsync();
        }

        [HttpGet("{id}")]
        [Authorize]
        public async Task<ActionResult<Products.Models.Products>> GetById(int id)
        {
            // EF translates this to: SELECT * FROM Products WHERE Id = @id
            var product = await _context.Products.FindAsync(id);

            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found." });

            return Ok(product);
        }


        [HttpPost]
        [Authorize]
        public async Task<ActionResult<Products.Models.Products>> Create([FromBody] Products.Models.Products product)
        {
            // EF translates this to: INSERT INTO Products (...)
            _context.Products.Add(product);
            await _context.SaveChangesAsync(); // Commit to DB

            // Returns 201 Created with Location header pointing to the new resource
            return CreatedAtAction(nameof(GetById), new { id = product.Id }, product);
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<ActionResult<Products.Models.Products>> Delete(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
                return NotFound(new { message = $"Product with ID {id} not found." });

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new { message = $"Product with ID {id} deleted successfully." });
        }

        [HttpPost("Bulk-add")]
        [Authorize]
        public async Task<ActionResult> BulkAdd([FromBody] List<Products.Models.Products> products)
        {
            if(products == null || products.Count == 0)
                return BadRequest(new { message = "Product list cannot be empty." });

            _context.Products.AddRangeAsync(products);
            await _context.SaveChangesAsync();
            return Ok(new { message = $"{products.Count} products added successfully." });
        }

    }
}







