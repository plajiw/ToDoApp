using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ToDoApp.Server.Data;
using ToDoApp.Server.Models;

namespace ToDoApp.Server.Controllers;

[Route("api/[controller]")]
[ApiController]
public class TodoController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public TodoController(ApplicationDbContext context)
    {
        _context = context;
    }

    // Obter todas as tarefas
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        return Ok(await _context.TodoItems.ToListAsync());
    }

    // Adicionar uma nova tarefa
    [HttpPost]
    public async Task<IActionResult> Post(TodoItem todoItem)
    {
        _context.TodoItems.Add(todoItem);
        await _context.SaveChangesAsync();

        // Retornar o objeto completo (com ID atualizado)
        return CreatedAtAction(nameof(Get), new { id = todoItem.Id }, todoItem);
    }

    // Atualizar o status de conclusão de uma tarefa
    [HttpPut("{id}/status")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] bool isCompleted)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound(new { message = "Tarefa não encontrada" });
        }

        todoItem.IsCompleted = isCompleted;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Put(int id, [FromBody] TodoItem updatedTodo)
    {
        var existingTodo = await _context.TodoItems.FindAsync(id);
        if (existingTodo == null)
        {
            return NotFound(new { message = "Tarefa não encontrada" });
        }

        existingTodo.Title = updatedTodo.Title;
        existingTodo.IsCompleted = updatedTodo.IsCompleted;

        await _context.SaveChangesAsync();
        return NoContent();
    }


    // Excluir uma tarefa
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var todoItem = await _context.TodoItems.FindAsync(id);
        if (todoItem == null)
        {
            return NotFound(new { message = "Tarefa não encontrada" });
        }

        _context.TodoItems.Remove(todoItem);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}
