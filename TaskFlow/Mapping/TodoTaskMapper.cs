using Riok.Mapperly.Abstractions;
using TaskFlow.DTOs.TodoTasks;
using TaskFlow.Entities;

namespace TaskFlow.Mapping;

[Mapper]
public partial class TodoTaskMapper
{
    public partial ReadTaskDto ToReadDto(TodoTask source);

    [MapperIgnoreTarget(nameof(TodoTask.Id))]
    [MapperIgnoreTarget(nameof(TodoTask.IsCompleted))]
    [MapperIgnoreTarget(nameof(TodoTask.CreatedAt))]
    public partial TodoTask ToEntities(CreateTaskDto source);

    [MapperIgnoreTarget(nameof(TodoTask.Id))]
    [MapperIgnoreTarget(nameof(TodoTask.IsCompleted))]
    [MapperIgnoreTarget(nameof(TodoTask.CreatedAt))]
    public partial void UpdateEntity(UpdateTaskDto source, TodoTask target);
}