using todo.DTOs;

namespace todo.Services;

public interface ITaskService
{
    Task<List<TaskResponseDto>> GetMineAsync(int userId);
    Task<List<TaskResponseDto>> GetAllAsync();
    Task<TaskResponseDto> GetByIdAsync(int id, int userId, bool isAdmin);
    Task<TaskResponseDto> CreateAsync(TaskCreateDto dto, int userId);
    Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto, int userId, bool isAdmin);
    Task DeleteAsync(int id, int userId, bool isAdmin);
}