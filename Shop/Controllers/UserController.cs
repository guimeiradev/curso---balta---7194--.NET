using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Models;
using Shop.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers {
    [Route("v1/users")]
    
    public class UserController : Controller {


        [HttpGet]
        [Route("")]
        [Authorize(Roles ="manager")]

      public async Task<ActionResult<List<User>>>Get([FromServices] DataContext context) {

            var users = await context.Users.AsNoTracking().ToListAsync();
                        
            return users;
                        
        }
        
        [HttpPost]
        [Route("")]
        [AllowAnonymous]
     

        public async Task<ActionResult<User>> Post([FromServices] DataContext context, [FromBody] User model) {

            // Verifica se os dados são válidos

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            try {

                // Força o usuario a ser sempre "funcionario"

                model.Role = "employee";

                context.Users.Add(model);
                await context.SaveChangesAsync();

                //Esconder a senha

                model.Passoword = "";
                return model;

            } catch (Exception) {

                return BadRequest(new { message = "não foi possível criar o usuário" });
            }
        }

        [HttpPost]
        [Route("login")]

        public async Task<ActionResult<dynamic>> Authenticate([FromBody] User model, [FromServices] DataContext context) {

            var user = await context.Users.AsNoTracking()
                .Where(x => x.Username == model.Username && x.Passoword == model.Passoword)
                .FirstOrDefaultAsync();

            if (user == null)
                return NotFound(new { message = "Usuario ou senha inválidos" });

            var tokem = TokenService.GenerateToken(user);

            // Esconder a senha

            user.Passoword = "";

            return new {
                user = user,
                token = tokem
            };
        }

        [HttpPut]
        [Route("{id:int}")]
        [Authorize(Roles ="manager")]

        public async Task<ActionResult<User>> Put([FromServices] DataContext context, int id,[FromBody] User model) {

            // Verificar se os dados são válidos

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            // Verifica se o ID informado é o mesmo do modelo

            if (id != model.Id)
                return NotFound(new { message = "Usuário não encontrado" });

            try {
                context.Entry(model).State = EntityState.Modified;
                await context.SaveChangesAsync();
                return model;
            
            } catch (Exception) {

                return BadRequest(new { message = "Não foi possível atualizar o usuario " });

            }
        }
    }
}
