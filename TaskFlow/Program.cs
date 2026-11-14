using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;
using TaskFlow.Infrastructure;
using TaskFlow.Mapping;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddRouting(option => option.LowercaseUrls = true);
builder.Services.AddDbContext<TasksDbContext>(optionsBuilder =>
    optionsBuilder.UseNpgsql(builder.Configuration.GetConnectionString("TaskFlowConnection")));
builder.Services.AddScoped<TodoTaskMapper>();
builder.Services.AddOpenApi();

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
