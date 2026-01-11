using System.Net;
using System.Security.Claims;
using Gantt_Chart_Backend.Services.Implementations;
using Microsoft.AspNetCore.Authorization;
using System.Linq;

namespace Gantt_Chart_Backend.Auth;

public class PermissionRequirementsHandler(IServiceScopeFactory serviceScopeFactory) : AuthorizationHandler<PermissionRequirements>
{
    protected override async Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        PermissionRequirements requirement)
    {
        var userName = context.User.Claims
            .FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier)?.Value;
        
        using var scope = serviceScopeFactory.CreateScope();
        
        var usersService = scope.ServiceProvider.GetService<UsersService>();
        var permissions = await usersService.GetUserPermissionsByName(userName);

        if (permissions.Any(p => p.Name == requirement.Permission))
        {
            context.Succeed(requirement);
        }
    }
}