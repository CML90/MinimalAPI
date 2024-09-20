namespace RestAPI.Models;

/// <summary>
/// This class sets the format and columns of the todo.
/// </summary>
public class Todo
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public DateTime DueDate { get; set; }
    public bool IsCompleted { get; set; }
}