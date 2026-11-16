using FluentValidation;
using Microsoft.AspNetCore.Mvc;
using TaskFlow.Infrastructure;
using Microsoft.EntityFrameworkCore;
using TaskFlow.DTOs.TodoTasks;
using TaskFlow.Extensions;
using TaskFlow.Filters;
using TaskFlow.Mapping;

namespace TaskFlow.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TasksController(
    TasksDbContext taskDbContext,
    TodoTaskMapper mapper) : ControllerBase
{
    [HttpGet("{id:int}", Name = "GetById")]
    [ProducesResponseType(typeof(ReadTaskDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadTaskDto>> GetById(int id)
    {
        var task = await taskDbContext.TodoTasks.FindAsync(id);

        if (task is null)
        {
            return Problem(
                detail: $"A task with id {id} has not been found",
                statusCode: StatusCodes.Status404NotFound);
        }

        return Ok(mapper.ToReadDto(task));
    }

    [HttpGet]
    [ProducesResponseType(typeof(List<ReadTaskDto>), statusCode: StatusCodes.Status200OK)]
    public async Task<ActionResult<List<ReadTaskDto>>> GetAll()
    {
        var todoTasks = await taskDbContext.TodoTasks.ToListAsync();

        var readDtos = todoTasks.Select(mapper.ToReadDto)
            .ToList();

        return Ok(readDtos);
    }

    [HttpPost]
    [ServiceFilter(typeof(ValidationFilters<CreateTaskDto>))]
    [ProducesResponseType(typeof(ReadTaskDto), statusCode: StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<ReadTaskDto>> Create(CreateTaskDto createDto)
    {
        var todoTask = mapper.ToEntities(createDto);

        taskDbContext.TodoTasks.Add(todoTask);
        await taskDbContext.SaveChangesAsync();

        return CreatedAtRoute(
            nameof(GetById),
            new { todoTask.Id },
            mapper.ToReadDto(todoTask));
    }

    [HttpPut("{id:int}")]
    [ServiceFilter(typeof(ValidationFilters<UpdateTaskDto>))]
    [ProducesResponseType(typeof(ReadTaskDto), statusCode: StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ProblemDetails), statusCode: StatusCodes.Status404NotFound)]
    public async Task<ActionResult<ReadTaskDto>> Update(int id, UpdateTaskDto updateDto)
    {
        var existingTask = await taskDbContext.TodoTasks.FindAsync(id);

        if (existingTask is null)
        {
            return Problem(
                detail: $"A task with id {id} has not been found.",
                statusCode: StatusCodes.Status404NotFound);
        }

        mapper.UpdateEntity(updateDto, existingTask);

        await taskDbContext.SaveChangesAsync();

        return Ok(mapper.ToReadDto(existingTask));
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
                statusCode: StatusCodes.Status404NotFound);
        }

        taskDbContext.Remove(todoTask);
        await taskDbContext.SaveChangesAsync();

        return NoContent();
    }
}