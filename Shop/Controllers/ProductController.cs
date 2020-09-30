using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers {
    [Route("products")]
    public class ProductController : ControllerBase {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]

        public async Task<ActionResult<List<Product>>> Get([FromServices] DataContext context) {

            var products = await context.Products.Include(x => x.Category).AsNoTracking().ToListAsync();

            // Include : Ele tem a funcão de fazer no sql e buscar as categorias.

            return products;

        }

        [HttpGet("{id:int}")]
        [AllowAnonymous]

        public async Task<ActionResult<Product>> GetById([FromServices] DataContext context, int id) {

            var product = await context.Products.Include(x => x.Category).AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

            return product;
        }

        [HttpGet] // products/categories/1
        [Route("categories/{id:int}")]
        [AllowAnonymous]

        public async Task<ActionResult<List<Product>>> GetByCategory([FromServices] DataContext context, int id) {

            var products = await context.Products.Include(x => x.Category).AsNoTracking().Where(x => x.Category.Id == id).ToListAsync();

            return products;

        }

        [HttpPost]
        [Route("")]
        [Authorize(Roles = "employee")]

        public async Task<ActionResult<Product>> Post([FromServices] DataContext context, [FromBody] Product model) {

            if (ModelState.IsValid) {

                context.Products.Add(model);

                await context.SaveChangesAsync();

                return model;


            } else {

                return BadRequest(ModelState);
            }
        }
    }
}
