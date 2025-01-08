using System.ComponentModel.DataAnnotations;

namespace ToDoApp.Server.Models;

public class TodoItem
{
    // Identificador
    [Key]
    public int Id { get; set; }

    // Nome
    [Required]
    public string Title { get; set; } = string.Empty;

    // Status
    public bool IsCompleted { get; set; }
}
