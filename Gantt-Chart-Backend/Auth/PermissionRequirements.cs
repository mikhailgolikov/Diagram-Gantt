using Microsoft.AspNetCore.Authorization;

namespace Gantt_Chart_Backend.Auth;

public class PermissionRequirements : IAuthorizationRequirement
{
    public string Permission;
    
    public PermissionRequirements(string permission)
    {
        Permission = permission;
    }
}