using System.Security.Claims;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gantt_Chart_Backend.Controllers;


//[Authorize]
[Route("api/teams")]
[ApiController]
public class ProjectStuffController : ControllerBase
{
    private readonly ITeamService _teamService;
    
    public ProjectStuffController(ITeamService teamService)
    {
        _teamService = teamService;
    }
    
    private Guid GetCurrentUserId()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId) 
            || !Guid.TryParse(userId, out var guid))
            throw new UnauthorizedAccessException();

        return guid;
    }
    
    
    [HttpPost]
    public async Task<IActionResult> CreateTeam(
        [FromBody] TeamDto team)
    {
        await _teamService.CreateTeam(team);
        return Ok();
    }

    [HttpPost]
    [Route("{teamId}/{memberId}")]
    public async Task<IActionResult> AddTeamMember(
        [FromRoute] Guid teamId,
        [FromRoute] Guid memberId)
    {
        await _teamService.AddTeamMember(teamId, memberId);
        return Ok();
    }
    
    [HttpDelete]
    [Route("{teamId}/{memberId}")]
    public async Task<IActionResult> RemoveTeamMember(
        [FromRoute] Guid teamId,
        [FromRoute] Guid memberId)
    {
        await _teamService.RemoveTeamMember(teamId, memberId);
        return Ok();
    }
}
