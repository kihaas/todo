using System.ComponentModel.DataAnnotations;

namespace todo.DTOs;

public class RegisterDto
{
    [Required(ErrorMessage = "Логин обязателен")]
    [StringLength(30, MinimumLength = 3, ErrorMessage = "Логин должен быть от 3 до 30 символов")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email обязателен")]
    [EmailAddress(ErrorMessage = "Некорректный email")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Пароль обязателен")]
    [StringLength(100, MinimumLength = 6, ErrorMessage = "Пароль должен быть от 6 до 100 символов")]
    public string Password { get; set; } = string.Empty;
}