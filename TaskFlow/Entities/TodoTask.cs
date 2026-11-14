using TaskFlow.Enums;

namespace TaskFlow.Entities;

public class TodoTask
{
    public int Id { get; set; }
    public required string Title { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public TaskPriority Priority { get; set; } = TaskPriority.Medium;
    public DateTime CreatedAt { get; init; } = DateTime.UtcNow;
    public DateTime? DueDate { get; set; }
}