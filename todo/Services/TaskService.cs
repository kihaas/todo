using Microsoft.EntityFrameworkCore;
using todo.Data;
using todo.DTOs;
using todo.Exceptions;
using todo.Models;

namespace todo.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _db;

    public TaskService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<TaskResponseDto>> GetMineAsync(int userId)
    {
        return await _db.Tasks
            .Where(t => t.UserId == userId)
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => ToDto(t))
            .ToListAsync();
    }

    public async Task<List<TaskResponseDto>> GetAllAsync()
    {
        return await _db.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => ToDto(t))
            .ToListAsync();
    }

    public async Task<TaskResponseDto> GetByIdAsync(int id, int userId, bool isAdmin)
    {
        var task = await FindAccessibleAsync(id, userId, isAdmin);
        return ToDto(task);
    }

    public async Task<TaskResponseDto> CreateAsync(TaskCreateDto dto, int userId)
    {
        var task = new TaskItem
        {
            Title = dto.Title,
            Description = dto.Description,
            UserId = userId
        };

        _db.Tasks.Add(task);
        await _db.SaveChangesAsync();

        return ToDto(task);
    }

    public async Task<TaskResponseDto> UpdateAsync(int id, TaskUpdateDto dto, int userId, bool isAdmin)
    {
        var task = await FindAccessibleAsync(id, userId, isAdmin);

        task.Title = dto.Title;
        task.Description = dto.Description;
        task.IsCompleted = dto.IsCompleted;

        await _db.SaveChangesAsync();
        return ToDto(task);
    }

    public async Task DeleteAsync(int id, int userId, bool isAdmin)
    {
        var task = await FindAccessibleAsync(id, userId, isAdmin);

        _db.Tasks.Remove(task);
        await _db.SaveChangesAsync();
    }

    private async Task<TaskItem> FindAccessibleAsync(int id, int userId, bool isAdmin)
    {
        var task = await _db.Tasks.FirstOrDefaultAsync(t => t.Id == id);

        if (task == null)
            throw new NotFoundException($"Задача с id {id} не найдена");

        if (task.UserId != userId && !isAdmin)
            throw new ForbiddenException("Это чужая задача");

        return task;
    }

    private static TaskResponseDto ToDto(TaskItem t)
    {
        return new TaskResponseDto
        {
            Id = t.Id,
            Title = t.Title,
            Description = t.Description,
            IsCompleted = t.IsCompleted,
            CreatedAt = t.CreatedAt,
            UserId = t.UserId
        };
    }
}