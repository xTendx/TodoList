using TaskFlow.Enums;

namespace TaskFlow.DTOs.TodoTasks;

public record UpdateTaskDto
{
    public required string Title { get; init; }
    public string? Description { get; init; }
    public TaskPriority? Priority { get; init; }
    public DateTime? DueDate { get; init; }
}