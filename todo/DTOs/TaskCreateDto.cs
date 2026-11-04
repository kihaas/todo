using System.ComponentModel.DataAnnotations;

namespace todo.DTOs;

public class TaskCreateDto
{
    [Required(ErrorMessage = "Название обязательно")]
    [StringLength(100, MinimumLength = 1, ErrorMessage = "Название до 100 символов")]
    public string Title { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "Описание до 500 символов")]
    public string? Description { get; set; }
}