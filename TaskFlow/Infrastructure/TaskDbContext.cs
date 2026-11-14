using Microsoft.EntityFrameworkCore;
using TaskFlow.Entities;


namespace TaskFlow.Infrastructure;

public class TasksDbContext(DbContextOptions<TasksDbContext> options) : DbContext(options)
{
    public DbSet<TodoTask> TodoTasks => Set<TodoTask>();
}