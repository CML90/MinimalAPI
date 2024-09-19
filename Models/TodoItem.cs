/// <summary>
/// 
/// </summary>
namespace RestAPI.Models; //must be === name of Project (.csproj file)


//keep these like this
// public record Todo(int Id, string Name, DateTime DueDate, bool IsCompleted);

// interface ITaskService
// {
//     Todo? GetTodoById(int id);
//     List<Todo> GetTodos();
//     void DeleteTodoById(int id);
//     Todo AddTodo(Todo task);
// }

/// <summary
/// This class sets the format and columns of the todo.
/// </summary>
public class Todo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}