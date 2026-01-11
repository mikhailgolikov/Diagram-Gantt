using Gantt_Chart_Backend.Auth;
using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gantt_Chart_Backend.Services.Implementations;

public class TeamService : ITeamService
{
    private readonly GanttPlatformDbContext _dbcontext;

    public TeamService(GanttPlatformDbContext dbcontext)
    {
        _dbcontext = dbcontext;
    }
    
    public async Task<Guid> CreateTeam(TeamDto teamDto)
    {
        var newTeam = new Team
        (
            name: teamDto.Name,
            teamDto.LeaderId,
            teamDto.ProjectId
        ); 
        
        if (teamDto.Members != null)
            newTeam.Performers.AddRange(teamDto.Members);
        
        _dbcontext.Teams.Add(newTeam);
        await _dbcontext.SaveChangesAsync();
        
        return newTeam.Id;
    }

    public async Task AddTeamMember(Guid teamId, Guid memberId)
    {
        var team = _dbcontext.Teams
            .FirstOrDefault(t => t.Id == teamId)
            ?? throw new NotFoundException();
        
        var user =  _dbcontext.ProjectMembers
            .FirstOrDefault(u => u.Id == memberId)
            ??  throw new NotFoundException();
        
        team.Performers.Add(user);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task RemoveTeamMember(Guid teamId, Guid memberId)
    {
        var team = _dbcontext.Teams
           .FirstOrDefault(t => t.Id == teamId) 
           ?? throw new NotFoundException();

        var user =  _dbcontext.ProjectMembers
           .FirstOrDefault(u => u.Id == memberId) 
           ??  throw new NotFoundException();
        
        team.Performers.Remove(user);
        await _dbcontext.SaveChangesAsync();
    }

    public async Task AddUserToProject(Guid userId, Guid projectId)
    {
        var project = _dbcontext.Projects
             .Include(p => p.Members)
             .FirstOrDefault(t => t.Id == projectId) 
             ?? throw new NotFoundException("ProjectNotFound");
        
        var user =  _dbcontext.Users
              .FirstOrDefault(u => u.Id == userId)
              ??  throw new NotFoundException("UserNotFound");

        if (project.Members.Any(u => u.Id == userId))
            return;
        
        var pm = new ProjectMember
        {
            Id = user.Id,
            ProjectId = projectId,
            Role = Role.Member,
            User = user,
            Project = project
        };
        
        project.Members.Add(pm);

        user.Roles.Add(pm);
        
        await _dbcontext.SaveChangesAsync();
    }

    public async Task RemoveUserFromProject(Guid userId, Guid projectId)
    {
        var project = _dbcontext.Projects
            .FirstOrDefault(t => t.Id == projectId) 
            ?? throw new NotFoundException();
        
        var user =  _dbcontext.ProjectMembers
            .FirstOrDefault(u => u.Id == userId)
            ??  throw new NotFoundException();
        
        project.Members.Remove(user);
        
        await _dbcontext.SaveChangesAsync();
    }

    public async Task SetUserRoleInProject(Guid userId, Guid projectId, Role role)
    {
        var user = _dbcontext.ProjectMembers
            .FirstOrDefault(u => u.Id == userId &&  u.ProjectId == projectId)
            ?? throw new NotFoundException();
        
        user.Role = role;

        user.Permissions = role switch
        {
            Role.Admin => GetAdminPermissions(),
            Role.Member => GetMemberPermissions()
        };
        
        await _dbcontext.SaveChangesAsync();
    }

    private ICollection<Permission> GetMemberPermissions()
    {
        var permissions = new List<Permission>();
        
        permissions.Add(new Permission(Permissions.CreateProject));
        permissions.Add(new Permission(Permissions.CreateTask));
        permissions.Add(new Permission(Permissions.ReadTask));
        permissions.Add(new Permission(Permissions.UpdateTask));
        permissions.Add(new Permission(Permissions.DeleteTask));
        permissions.Add(new Permission(Permissions.SetPerformer));
        permissions.Add(new Permission(Permissions.SetTaskDuration));
        permissions.Add(new Permission(Permissions.SetDependence));
        permissions.Add(new Permission(Permissions.AddComment));
        permissions.Add(new Permission(Permissions.RemoveComment));
        permissions.Add(new Permission(Permissions.UpdateTaskStatus));
        permissions.Add(new Permission(Permissions.CreateTeam));
        permissions.Add(new Permission(Permissions.ReadTeam));
        permissions.Add(new Permission(Permissions.UpdateTeam));
        permissions.Add(new Permission(Permissions.DeleteTeam));
        permissions.Add(new Permission(Permissions.DeleteProject));

        return permissions;
    } 
    
    private ICollection<Permission> GetAdminPermissions()
    {
        var permissions =  new List<Permission>();
        
        permissions.Add(new Permission(Permissions.CreateProject));
        permissions.Add(new Permission(Permissions.CreateTask));
        permissions.Add(new Permission(Permissions.ReadTask));
        permissions.Add(new Permission(Permissions.UpdateTask));
        permissions.Add(new Permission(Permissions.DeleteTask));
        permissions.Add(new Permission(Permissions.SetPerformer));
        permissions.Add(new Permission(Permissions.SetTaskDuration));
        permissions.Add(new Permission(Permissions.SetDependence));
        permissions.Add(new Permission(Permissions.AddComment));
        permissions.Add(new Permission(Permissions.RemoveComment));
        permissions.Add(new Permission(Permissions.UpdateTaskStatus));
        permissions.Add(new Permission(Permissions.CreateTeam));
        permissions.Add(new Permission(Permissions.ReadTeam));
        permissions.Add(new Permission(Permissions.UpdateTeam));
        permissions.Add(new Permission(Permissions.DeleteTeam));
        permissions.Add(new Permission(Permissions.DeleteProject));
        permissions.Add(new Permission(Permissions.UpdateProject));
        permissions.Add(new Permission(Permissions.SetRootTask));
        permissions.Add(new Permission(Permissions.SetProjectDuration));
        permissions.Add(new Permission(Permissions.AddUser));
        permissions.Add(new Permission(Permissions.RemoveUser));
        permissions.Add(new Permission(Permissions.SetUserRole));

        return permissions;

    } 
}