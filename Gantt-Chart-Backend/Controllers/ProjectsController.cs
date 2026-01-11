using System.Security.Claims;
using Gantt_Chart_Backend.Auth;
using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gantt_Chart_Backend.Controllers;

//[Authorize]
[ApiController]
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;
    private readonly IInviteService _inviteService;
    private readonly ITeamService _teamService;

    public ProjectsController(
        IProjectService projectService, 
        ITeamService teamService,
        IInviteService inviteService
        )
    {
        _projectService = projectService;
        _inviteService = inviteService;
        _teamService = teamService;
    }

    private Guid GetCurrentUserId()
    {
        //var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var userId = User.FindFirstValue("userId");
        if (string.IsNullOrEmpty(userId) 
            || !Guid.TryParse(userId, out var guid))
            throw new UnauthorizedAccessException();
        
        return guid;
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateProject(
        [FromBody] ProjectCreateDto project)
    {
        return Ok(await _projectService.CreateProject(project));
    }
    
    [HttpGet]
    public async Task<IActionResult> GetUserProjects(
        [FromQuery] Guid userId)
    {
        Console.WriteLine(GetCurrentUserId());
        try
        {
            return Ok(await _projectService.GetUserProjects(userId));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet]
    [Route("{projectId:guid}")]
    public async Task<IActionResult> GetProjectInfo(
        [FromRoute] Guid projectId)
    {
        //var userId = GetCurrentUserId();
        try
        {
            return Ok(await _projectService.GetFullProjectInfo(projectId, Guid.Empty));
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ForbidException ex)
        {
            return Forbid(ex.Message);
        }
    }

    [HttpPatch]
    //[Authorize(Permissions.UpdateProject)]
    [Route("{projectId:guid}")]
    public async Task<IActionResult> UpdateProject(
        [FromRoute] Guid projectId,
        [FromBody] ProjectDto newProject)
    {
        await _projectService.UpdateProject(projectId, newProject);
        return Ok();
    }

    [HttpDelete]
    //[Authorize(Permissions.DeleteProject)]
    [Route("{projectId:guid}")]
    public async Task<IActionResult> DeleteProject(
        [FromRoute] Guid projectId)
    {
        Guid userId = GetCurrentUserId();
        await _projectService.DeleteProject(projectId, userId);
        
        return NoContent();
    }

    [HttpPatch]
    //[Authorize(Permissions.SetRootTask)]
    [Route("{projectId:guid}/root")]
    public async Task<IActionResult> SetProjectRootTask(
        [FromRoute] Guid projectId,
        [FromQuery] Guid taskId
        )
    {
        await _projectService.SetProjectRootTask(projectId, taskId);
        return Ok();
    }
    
    [HttpPost]
    //[Authorize(Permissions.AddUser)]
    [Route("{projectId:guid}/members")]
    public async Task<IActionResult> AddUserToProject(
        [FromQuery] Guid userId,
        [FromRoute] Guid projectId)
    {
        await _teamService.AddUserToProject(projectId, userId);
        return Ok();
    }
    
    [HttpDelete]
    //[Authorize(Permissions.RemoveUser)]
    [Route("{projectId:guid}/members/{userId:guid}")]
    public async Task<IActionResult> RemoveUserFromProject(
        [FromRoute] Guid userId,
        [FromRoute] Guid projectId)
    {
        try
        {
            await _teamService.RemoveUserFromProject(userId, projectId);
            return NoContent();
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpPatch]
    //[Authorize(Permissions.SetUserRole)]
    [Route("{projectId:guid}/members/{userId:guid}")]
    public async Task<IActionResult> SetUserRoleInProject(
        [FromRoute] Guid userId,
        [FromRoute] Guid projectId,
        [FromBody] Role role)
    {
        await _teamService.SetUserRoleInProject(userId, projectId, role);
        return NoContent();
    }

    [HttpGet]
    [Route("/invite/{inviteCode}")]
    public async Task<IActionResult> InviteUserToProject(
        [FromRoute] string inviteCode,
        [FromQuery] Guid userId)
    {
        try
        {
            await _inviteService.AddUserToProjectByInviteCode(userId, inviteCode);
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}