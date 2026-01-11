using System.Security.Claims;
using Gantt_Chart_Backend.Auth;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gantt_Chart_Backend.Controllers;

[Route("api/tasks")]
[ApiController]
public class TaskController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TaskController(ITaskService taskService)
    {
        _taskService = taskService;
    }
    
    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) || !Guid.TryParse(userId, out var guid))
        {
            throw new UnauthorizedAccessException();
        }

        return guid;
    }
    
    [HttpGet]
    //[Authorize(Permissions.ReadTask)]
    [Route("{taskId}")]
    public async Task<IActionResult> GetTaskInfo(
        [FromRoute] Guid taskId)
    {
        return Ok(await _taskService.GetTask(taskId));
    }
    
    [HttpPost]
    public async Task<IActionResult> AddTask(ProjectTaskDto dto)
    {
        try
        {
            return Ok(await _taskService.AddTask(dto));
        }
        catch (Exception ex)
        {
            return Ok(ex.Message);
        }
    }
    
    [HttpDelete]
    [Route("{taskId}")]
    public async Task<IActionResult> DeleteTask([FromRoute] Guid taskId)
    {
        await _taskService.DeleteTask(taskId);
        return Ok();
    }
    
    [HttpPatch]
    [Route("{taskId}")]
    public async Task<IActionResult> UpdateTask(
        [FromRoute] Guid taskId,
        [FromBody] ProjectTaskDto taskDto)
    {
        await _taskService.UpdateTask(taskDto, taskId);
        return Ok();
    }
    
    [HttpPatch]
    [Route("{taskId}/status")]
    public async Task<IActionResult> SetTaskStatus(
        [FromRoute] Guid taskId,
        [FromBody] bool status)
    {
        try
        {
            var (message, success) = await _taskService.SetTaskStatus(taskId, status);
            
            return Ok(new {message, success});
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPost]
    [Route("{taskId}/dependence")]
    public async Task<IActionResult> AddTaskDependence(
        [FromBody] DependenceDto dep)
    {
        try
        {
            await _taskService.AddTaskDependence(dep);
            return Ok();
        }
        catch (InvalidOperationException ex)
        {
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound();
        }
        catch (DependenceAlreadyExistsException ex)
        {
            return Ok();
        }
    }

    [HttpDelete]
    [Route("{taskId}/dependence")]
    public async Task<IActionResult> RemoveTaskDependence(
        [FromBody] DependenceDto dep)
    {
        try
        {
            await _taskService.RemoveTaskDependence(dep);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound();
        }
    }
    
    [HttpPost]
    [Route("{taskId}/performers/users")]
    public async Task<IActionResult> AddTaskPerformerUser(
        [FromRoute] Guid taskId,
        [FromQuery] Guid id)
    {
        await _taskService.AddTaskPerformer(taskId, id, 0);
        return Ok();
    }

    [HttpDelete]
    [Route("{taskId}/performers/users")]
    public async Task<IActionResult> RemoveTaskPerformersUser(
        [FromRoute] Guid taskId,
        [FromQuery] Guid id)
    { 
        await _taskService.RemoveTaskPerformer(taskId, id, 0);
        return Ok();
    }
    
    [HttpPost]
    [Route("{taskId}/performers/teams")]
    public async Task<IActionResult> AddTaskPerformersTeam(
        [FromRoute] Guid taskId,
        [FromQuery] Guid id)
    {
        await _taskService.AddTaskPerformer(taskId, id, 1);
        return Ok();
    }

    [HttpDelete]
    [Route("{taskId}/performers/teams")]
    public async Task<IActionResult> RemoveTaskPerformersTeam(
        [FromRoute] Guid taskId,
        [FromQuery] Guid id)
    { 
        await _taskService.RemoveTaskPerformer(taskId, id, 1);
        return Ok();
    }




    /*[HttpGet]
    [Route("{taskId}/comments")]
    public async Task<IActionResult> GetTaskComments(
        [FromRoute] Guid taskId)
    {
        try
        {
            return Ok(await _taskService.GetTaskComment(taskId));
        }
        catch (NotFoundException ex)
        {
            return NotFound();
        }
    }*/
    
    [HttpPost]
    [Route("{taskId}/comments")]
    public async Task<IActionResult> AddTaskComment(
        [FromRoute] Guid taskId,
        [FromBody] CommentDto commentDto)
    {
        try
        {
            return Ok(await _taskService.AddTaskComment(commentDto));
        }
        catch (NotFoundException ex)
        {
            return NotFound();
        }
    }
    
    [HttpDelete]
    [Route("{taskId}/comments")]
    public async Task<IActionResult> RemoveTaskComment(
        [FromRoute] Guid taskId,
        [FromQuery] Guid commentId)
    {
        try
        {
            await _taskService.RemoveTaskComment(taskId, commentId);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound();
        }
    }
}