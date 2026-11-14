using Microsoft.AspNetCore.Mvc;
using TaskFlow.Entities;
using TaskFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace TaskFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(
    TasksDbContext taskDbContext) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetById")]
    [ProducesResponseType(typeof(TodoTask), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoTask>> GetById(int id)
    {
        var task = await taskDbContext.TodoTasks.FindAsync(id);

        if (task is null)
        {
            return Problem(
                detail: $"A task with id {id} has not been found",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(task);
    }
    
    
    [HttpGet]
    [ProducesResponseType(typeof(List<TodoTask>), statusCode: StatusCodes.Status200OK)]
    public async Task<ActionResult<List<TodoTask>>> GetAll()
    {
        return Ok(await taskDbContext.TodoTasks.ToListAsync());
    }

    [HttpPost]
    [ProducesResponseType(typeof(TodoTask), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<TodoTask>> Create(TodoTask createTodoTask)
    {
        if (string.IsNullOrWhiteSpace(createTodoTask.Title))
        {
            return Problem(
                detail: "A task title cannot be empty.",
                statusCode:StatusCodes.Status400BadRequest);
        }

        if (createTodoTask.DueDate is DateTime dueData && 
            dueData < DateTime.UtcNow)
        {
            return Problem(
                detail: "A task due time is incorrect.",
                statusCode: StatusCodes.Status400BadRequest);
        }
        
        //TODO DTOs
        //Dont create a validation of CreatedAt

        var todoTask = new TodoTask
        {
            Title = createTodoTask.Title,
            Description = createTodoTask.Description,
            IsCompleted = false,
            Priority = createTodoTask.Priority,
            CreatedAt = DateTime.UtcNow,
            DueDate = createTodoTask.DueDate
        };

        taskDbContext.TodoTasks.Add(todoTask);
        await taskDbContext.SaveChangesAsync();

        return CreatedAtRoute(
            nameof(GetById),
            new { todoTask.Id },
            todoTask);
    }

    [HttpPut("{id:int}")]
    [ProducesResponseType(typeof(TodoTask), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<TodoTask>> Update(int id, TodoTask updateTodoTask)
    {
        if (string.IsNullOrWhiteSpace(updateTodoTask.Title))
        {
            return Problem(
                detail: "A task title cannot be empty.",
                statusCode: StatusCodes.Status400BadRequest);
        }

        if (updateTodoTask.DueDate is DateTime dateTime &&
            dateTime < DateTime.UtcNow)
        {
            return Problem(
                detail: "A task due time is inccorrect",
                statusCode: StatusCodes.Status400BadRequest);
        }
        
        var todoTask = await taskDbContext.TodoTasks.FindAsync(id);

        if (todoTask is null)
        {
            return Problem(
                detail: $"A task with id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        todoTask.Title = updateTodoTask.Title;
        todoTask.Description = updateTodoTask.Description;
        todoTask.Priority = updateTodoTask.Priority;
        todoTask.DueDate = updateTodoTask.DueDate;

        await taskDbContext.SaveChangesAsync();

        return Ok(todoTask);
    }

    [HttpPatch("{id:int}/complete")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Complete(int id)
    {
        var todoTask = await taskDbContext.TodoTasks.FindAsync(id);

        if (todoTask is null)
        {
            return Problem(
                detail: $"A task with id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        todoTask.IsCompleted = true;
        await taskDbContext.SaveChangesAsync();

        return NoContent();
    }
    

    [HttpDelete("{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Delete(int id)
    {
        var todoTask = await taskDbContext.TodoTasks.FindAsync(id);

        if (todoTask is null)
        {
            return Problem(
                detail: $"A task with id {id} has not been found.",
                statusCode:StatusCodes.Status404NotFound);
        }

        taskDbContext.Remove(todoTask);
        await taskDbContext.SaveChangesAsync();

        return NoContent();
    }
}

