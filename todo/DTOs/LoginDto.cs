using System.ComponentModel.DataAnnotations;

namespace todo.DTOs;

public class LoginDto
{
    [Required(ErrorMessage = "Логин обязателен")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    public string Password { get; set; } = string.Empty;
}