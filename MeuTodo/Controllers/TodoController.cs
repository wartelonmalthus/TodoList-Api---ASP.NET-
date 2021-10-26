using Microsoft.AspNetCore.Mvc;
using MeuTodo.Models;
using System.Collections.Generic;
using MeuTodo.Data;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MeuTodo.ViewModels;

namespace MeuTodo.Controllers
{
    [ApiController]
    [Route(template:"v1")]
    public class TodoController : ControllerBase
    {

        //GET ALL

        [HttpGet]
        [Route(template:"todos")]
        public async Task<IActionResult> GetAsync(
            [FromServices] 
            AppDbContext context
        ){

            var todos = await context.Todos.AsNoTracking().ToListAsync();

            return Ok(todos);
        }
        
        //GET BY ID

         [HttpGet]
        [Route(template:"todos/{id}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromServices] 
            AppDbContext context,
            [FromRoute]
            int id 
        ){

            var todo = await context.Todos.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id);

             return todo == null ? NotFound(): Ok(todo);

        }

        //POST 

        [HttpPost(template:"todos")]
        public async Task<IActionResult> PostAsync(
            [FromServices]
            AppDbContext context, 
            [FromBody]
            CreateTodoViewModel model
        ){

            if(!ModelState.IsValid)
            return BadRequest();

            var todo = new Todo{
                Title = model.Title,
                Done = false,

            };
            
            try
            {
                await context.Todos.AddAsync(todo);
                await context.SaveChangesAsync();
                return Created($"v1/todos/{todo.Id}", todo);
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }
        

        //PUT 

        [HttpPut(template:"todos/{id}")]
         public async Task<IActionResult> PutAsync(
            [FromServices]
            AppDbContext context, 
            [FromBody]
            CreateTodoViewModel model,
            [FromRoute]
            int id
        ){

            //Validacao do Objeto
            if(!ModelState.IsValid)
            return BadRequest();

            var todo = await context.Todos.FirstOrDefaultAsync(x=>x.Id == id);

            if(todo == null)
            return NotFound();
            
            try
            {
                todo.Title = model.Title;

                context.Todos.Update(todo);   
                await context.SaveChangesAsync();
                return Ok(todo);
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }
        

        //Delete

        [HttpDelete(template:"todos/{id}")]
        public async Task<IActionResult> DeleteAsync(
            [FromServices]
            AppDbContext context,
            [FromRoute]
            int id
        ){
            var todo = await context.Todos.FirstOrDefaultAsync(x => x.Id == id);

             if(todo == null)
            return NotFound();

            try
            {
                  context.Todos.Remove(todo);
                  await context.SaveChangesAsync();
                  return Ok();
            }
            catch (System.Exception)
            {
                
                throw;
            }

        }

    }
}