using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
namespace RestAPI.Models;

/// <summary>
/// This class inehrits from DbContext, allowing us to connect to the database.
/// </summary>
public class TodoContext : DbContext
{
    public TodoContext(DbContextOptions<TodoContext> options)
        : base(options)
    {
    }

    public DbSet<Todo> ToDos { get; set; } = null!;
}