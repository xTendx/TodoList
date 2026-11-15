using FluentValidation;
using TaskFlow.DTOs.TodoTasks;

namespace TaskFlow.Validators.TodoTasks;

public class UpdateTodoTaskDtoValidator : AbstractValidator<UpdateTaskDto>
{
    public UpdateTodoTaskDtoValidator()
    {
        RuleFor(dto => dto.Title)
            .NotEmpty()
            .MaximumLength(70)
            .WithMessage("A task title cannot be empty and can has 70 chars.");
        RuleFor(dto => dto.Priority)
            .IsInEnum()
            .WithMessage("Priority must be between 1 and 3.");
        RuleFor(dto => dto.DueDate)
            .InclusiveBetween(DateTime.UtcNow, DateTime.MaxValue);
    }
}