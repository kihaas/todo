using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using todo.DTOs;
using todo.Services;

namespace todo.Controllers;

[ApiController]
[Authorize]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly ITaskService _tasks;

    public TasksController(ITaskService tasks)
    {
        _tasks = tasks;
    }

    private int UserId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    private bool IsAdmin => User.IsInRole("Admin");

    [HttpGet]
    public async Task<ActionResult<List<TaskResponseDto>>> GetMine()
    {
        return Ok(await _tasks.GetMineAsync(UserId));
    }

    [HttpGet("all")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<List<TaskResponseDto>>> GetAll()
    {
        return Ok(await _tasks.GetAllAsync());
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> GetById(int id)
    {
        return Ok(await _tasks.GetByIdAsync(id, UserId, IsAdmin));
    }

    [HttpPost]
    public async Task<ActionResult<TaskResponseDto>> Create(TaskCreateDto dto)
    {
        var created = await _tasks.CreateAsync(dto, UserId);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TaskResponseDto>> Update(int id, TaskUpdateDto dto)
    {
        return Ok(await _tasks.UpdateAsync(id, dto, UserId, IsAdmin));
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        await _tasks.DeleteAsync(id, UserId, IsAdmin);
        return NoContent();
    }
}