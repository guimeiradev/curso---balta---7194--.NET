using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Shop.Controllers {
    [Route("v1/categories")]
    public class CategoryController : ControllerBase {

        [HttpGet]
        [Route("")]
        [AllowAnonymous]
        [ResponseCache(VaryByHeader ="User-Agent",Location = ResponseCacheLocation.Any, Duration = 30)]
   
        public async Task<ActionResult<Category>> Get([FromServices] DataContext context) {

            var categories = await context.Categories.AsNoTracking().ToListAsync();

            return Ok(categories);

        }


        [HttpGet]
        [Route("{id:int}")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Category>>> GetById(int id, [FromServices] DataContext context) {

            var category = await context.Categories.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);


            return Ok(category);
        }


        [HttpPost]
        [Route("")]
        [Authorize(Roles ="employee")]
        public async Task<ActionResult<List<Category>>> Post([FromBody] Category model, [FromServices] DataContext context) {

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Categories.Add(model); // Adcionando item no banco de dados 

                await context.SaveChangesAsync(); //Salavando as mudanças de forma assincrona 

                return Ok(model); //preenchendo o id automaticamente


            } catch (Exception) { // Da mais detalhes dos erros 

                return BadRequest(new { message = "não foi possivel criar categoria" });
            }
        }


        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Put([FromBody] Category model, [FromServices] DataContext context) {

            // Verifica se o ID informado é o mesmo do modelo (Validando modelo)

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {
                context.Entry<Category>(model).State = EntityState.Modified;
                await context.SaveChangesAsync();

                return Ok(model);

            } catch (DbUpdateConcurrencyException) { // Erro de concorrencia , não deixa o mesmo item ser atualizado novamente. 
                return BadRequest(new { message = "Este registro ja foi atualizado" });


            } catch (Exception) {

                return BadRequest(new { message = "Não foi possivel atualizar a categoria" });
            }

        }


        [HttpDelete]
        [Route("{id:int}")]
        [Authorize(Roles = "employee")]
        public async Task<ActionResult<List<Category>>> Delete(int id, [FromServices] DataContext context) {

            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);

            if (category == null)
                return NotFound(new { message = "Categoria não encontrada" });

            try {
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(category);

            } catch (Exception) {

                return BadRequest(new { message = "não foi possivel remover a categpria" });

            }
        }
    }
}
