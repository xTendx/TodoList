using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TaskFlow.DTOs.TodoTasks;
using TaskFlow.Infrastructure;
using TaskFlow.Mapping;
using TaskFlow.Validators.TodoTasks;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddDbContext<TasksDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("TaskFlowConnection")));
builder.Services.AddScoped<TodoTaskMapper>();
builder.Services.AddScoped<IValidator<CreateTaskDto>, CreateTodoTaskDtoValidator>();
builder.Services.AddScoped<IValidator<UpdateTaskDto>, UpdateTodoTaskDtoValidator>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();

    await DataSeeder.SeedAsync(app.Services);
}

app.UseHttpsRedirection();

app.MapControllers();

app.Run();
