using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using test_api.Models;

namespace test_api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ToDosController : ControllerBase
    {
        private readonly TestApiContext db;
        public ToDosController(TestApiContext context)
        {
            db = context;
        }

        // GET
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var todos = await db.Todos.ToArrayAsync();

            if (todos.Length == 0)
            {
                return NotFound();
            }

            return Ok(todos);
        }

        // POST 
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Title")] Todo todo)
        {
            todo.IsCompleted = false;
            try
            {
                if (ModelState.IsValid)
                {
                    db.Todos.Add(todo);
                    await db.SaveChangesAsync();
                }
            }
            catch (DataException /* dex */)
            {
                //Log the error (uncomment dex variable name and add a line here to write a log.
                ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
            }
            return Ok(todo);
        }

        //PUT
        [HttpPut]
        public async Task<IActionResult> PUT(Todo todo)
        {
            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            var existingTodo = await db.Todos.FirstAsync(t => t.Id == todo.Id);

            if (existingTodo != null)
            {
                try
                {

                    existingTodo.IsCompleted = todo.IsCompleted;
                    existingTodo.Title = todo.Title;

                    await db.SaveChangesAsync();
                }
                catch (DataException /* dex */)
                {
                    //Log the error (uncomment dex variable name and add a line here to write a log.
                    ModelState.AddModelError("", "Unable to save changes. Try again, and if the problem persists see your system administrator.");
                }
            }
            else
            {
                return NotFound();
            }

            return Ok(todo);
        }

        //Delete
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var todo = await db.Todos.FirstAsync(t => t.Id == id);

            if (todo != null)
            {
                db.Todos.Remove(todo);
                await db.SaveChangesAsync();
            }
            else
            {
                return BadRequest("No todo matches that Id");
            }

            return Ok(todo);
        }
    }
}
