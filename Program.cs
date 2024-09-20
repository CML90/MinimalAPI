using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Rewrite;
using RestAPI.Models;
using System.Runtime.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<TodoContext>(opt =>
    opt.UseInMemoryDatabase("TodoList"));

var app = builder.Build();

app.UseRewriter(new RewriteOptions().AddRedirect("tasks/(.*)", "todos/"));
app.Use(async (context, next) => 
{
    Console.WriteLine($"[{context.Request.Method}] {context.Request.Path} [{DateTime.UtcNow}] Started.");
    await next(context);
    Console.WriteLine($"[{context.Request.Method}] {context.Request.Path} [{DateTime.UtcNow}] Finished.");
});

/// <summary>
/// This middleware gets all the todo items in the table.
/// </summary>
/// <param name="todo">TodoContext is class for DbContext.</param>
/// <remarks
/// ToDos is under DbSet
/// </remarks>
app.MapGet("/todos", async (TodoContext todo) => await todo.ToDos.ToListAsync());

/// <summary>
/// This middleware the one specified todo item.
/// </summary>
/// <param name="id">Todo item's id</param>
app.MapGet("/todos/{id}", async (int id, TodoContext todo) =>
    await todo.ToDos.FindAsync(id)
        is Todo task
            ? Results.Ok(task)
            : Results.NotFound());

/// <summary>
/// This middleware adds a given todo item (task) to the database.
/// </summary>
/// <param name="task">Individual todo item</param>
app.MapPost("/todos", async (Todo task, TodoContext todo) => 
{
    todo.ToDos.Add(task);
    await todo.SaveChangesAsync();
})
.AddEndpointFilter(async (context, next) => 
{
    var taskArgument = context.GetArgument<Todo>(0);
    var errors = new Dictionary<string, string[]>();
    if(taskArgument.DueDate < DateTime.UtcNow)
    {
        errors.Add(nameof(Todo.DueDate), ["Cannot have due date in the past"]);
    }
    if(taskArgument.IsCompleted)
    {
        errors.Add(nameof(Todo.IsCompleted), ["Cannot add completed todo"]);
    }

    if(errors.Count > 0){
        return Results.ValidationProblem(errors);
    }

    return await next(context);
});

/// <summary>
/// This middleware deletes a specific todo item.
/// </summary>
app.MapDelete("/todos/{id}", async (int id, TodoContext todo) => 
{
    if (await todo.ToDos.FindAsync(id) is Todo task)
    {
        todo.ToDos.Remove(task);
        await todo.SaveChangesAsync();
        return Results.NoContent();
    }
    return Results.NotFound();
});


app.Run();