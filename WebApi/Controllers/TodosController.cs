using Domain.Contracts;
using Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers; 

[ApiController]
[Route("[controller]")]

public class TodosController : ControllerBase {
    private ITodoHome todoHome;


    public TodosController(ITodoHome todoHome) {
        this.todoHome = todoHome;
    }
    
    [HttpGet]
    public async Task<ActionResult<ICollection<Todo>>> GetAll()
    {
        try
        {
            ICollection<Todo> todos = await todoHome.GetAsync();
            return Ok(todos);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
    
    [HttpPost]
    public async Task<ActionResult<Todo>> AddTodo([FromBody] Todo todo)
    {
        try
        {
            Todo added = await todoHome.AddAsync(todo);
            return Created($"/todos/{added.Id}", added);
        }
        catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<ActionResult<Todo>> GetTodo([FromRoute] int id) {
        try {
            Todo todo = await todoHome.GetById(id);
            return Ok(todo);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpDelete]
    [Route("{id:int}")]
    public async Task<ActionResult> Delete([FromRoute] int id) {
        try {
            await todoHome.DeleteAsync(id);
            return Ok();
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch]
    public async Task<ActionResult<Todo>> UpdateTodo([FromBody] Todo todo) {
        try {
            await todoHome.UpdateAsync(todo);
            return Ok(todo);
        }
        catch (Exception e) {
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet]
    [Route("search")]
    public async Task<ActionResult<ICollection<Todo>>> GetAll([FromQuery] int id, [FromQuery] bool isCompleted) {
        Console.WriteLine(id);
        Console.WriteLine(isCompleted);
        try {
            ICollection<Todo> collection = await todoHome.GetAsync(id, isCompleted);
            return Ok(collection);
        } catch (Exception e)
        {
            return StatusCode(500, e.Message);
        }
    }
}