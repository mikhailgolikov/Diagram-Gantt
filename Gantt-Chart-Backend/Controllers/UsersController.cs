using System.Security.Authentication;
using System.Security.Claims;
using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Gantt_Chart_Backend.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    public UsersController(IUsersService usersService)
    {
        _usersService = usersService;
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
    [Route("{userId}")]
    public async Task<IActionResult> GetUser(Guid userId)
    {
        return Ok();
    }

    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> RegisterUser(
        [FromBody] UserRequestDto userRequestDto)
    {
        try
        {
            return Ok(await _usersService.Register(userRequestDto));
        }
        catch (UserAlreadyExistsException ex)
        {
            return Conflict(ex.Message);
        } 
    }

    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(
        [FromBody]LoginUserRequest user
        //HttpContext httpContext
        )
    {
        try
        {
            var token = await _usersService.Login(user);
            //httpContext.
            Response.Cookies.Append("jwt-token", token);
            return Ok(token);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
    }

    [HttpPatch]
    [Route("{userId}/profile")]
    public async Task<IActionResult> UpdateProfile(
        [FromRoute] Guid userId,
        [FromBody] UpdateProfileDto userRequestDto
        )
    {
        try
        {
            await _usersService.UpdateUserData(userRequestDto, userId);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidCredentialsException ex)
        {
            return Unauthorized(ex.Message);
        }
        catch (ConflictException ex)
        {
            return Conflict(ex.Message);
        }
    }

    [HttpPatch]
    [Route("{userId}/password")]
    public async Task<IActionResult> UpdatePassword(
        [FromRoute] Guid userId,
        [FromBody] UpdateProfileDto dto
        )
    {
        try
        {
            await _usersService.UpdateUserPassword(dto, userId);
            return Ok();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidCredentialException ex)
        {
            return BadRequest(ex.Message);
        }
    }
}