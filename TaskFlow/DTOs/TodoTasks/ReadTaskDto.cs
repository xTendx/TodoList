using TaskFlow.Enums;

namespace TaskFlow.DTOs.TodoTasks;

public record ReadTaskDto
{
    public int Id { get; init; }
    public required string Title { get; init; }
    public string? Description { get; init; }
    public bool IsCompleted { get; init; }
    public TaskPriority? Priority { get; init; }
    public DateTime CreatedAt { get; init; }
    public DateTime? DueDate { get; init; }
}