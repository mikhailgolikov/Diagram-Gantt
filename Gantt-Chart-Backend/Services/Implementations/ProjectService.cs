using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gantt_Chart_Backend.Services.Implementations;

public class ProjectService : IProjectService
{
    private readonly GanttPlatformDbContext _dbcontext;
    private readonly IInviteService _inviteService;
    private readonly ITeamService _teamService;
    
    public ProjectService(
        GanttPlatformDbContext dbcontext,
        IInviteService inviteService,
        ITeamService teamService)
    {
        _teamService =  teamService;
        _inviteService = inviteService;   
        _dbcontext = dbcontext;
    }

    public async Task<Guid> CreateProject(ProjectCreateDto project)
    {
        var newProject = new Project
        {
            Id =  Guid.NewGuid(),
            Name = project.Name,
            CreatorId = project.CreatorId,
            RootTaskId = null,
            RootTask = null,
            DeadLine = project.DeadLine ??  DateTime.Now.AddDays(10),
            Members = new List<ProjectMember>(),
            Teams = new List<Team>(),
        };
        
        _dbcontext.Projects.Add(newProject);
        await _dbcontext.SaveChangesAsync();
        
        var rootTask = new ProjectTask
        (
            project.Name,
            newProject.Id,
            newProject.DeadLine.AddDays(-1),
            newProject.DeadLine
        );

        _dbcontext.Tasks.Add(rootTask);
        await  _dbcontext.SaveChangesAsync();
         
        newProject.RootTaskId = rootTask.Id;
        await _dbcontext.SaveChangesAsync();

        var firstCode = new InviteCode(newProject.Id, _inviteService.GenerateCode());
        await _dbcontext.InviteCodes.AddAsync(firstCode);
        await _dbcontext.SaveChangesAsync();
        
        await _teamService.AddUserToProject(project.CreatorId, newProject.Id);
        
        await _dbcontext.SaveChangesAsync();
            
        await _teamService.SetUserRoleInProject(project.CreatorId, newProject.Id, Role.Admin);
        
        return newProject.Id;
    }

    public async Task<ICollection<ProjectCardDto>> GetUserProjects(Guid userId)
    {
        var projects = await _dbcontext.Projects
            .AsNoTracking()
            .Include(p => p.Members)
            .ThenInclude(m => m.User)
            .Include(p => p.Creator)
            .Where(p =>
               p.Members
                   .Any(u => u.Id == userId))
            .Select(p => new ProjectCardDto(
                   p.Id,
                   p.Name ?? string.Empty,
                   p.Members != null? p.Members.Count : 0,
                   p.Creator.NickName ?? string.Empty,
                   _dbcontext.ProjectMembers 
                       .AsNoTracking()
                       .FirstOrDefault(u => u.Id == userId && u.ProjectId == p.Id).Role
               ))
               .ToListAsync();

        if (!projects.Any())
            throw new NotFoundException();
        
        /*var projectCards = projects
            .Select(p => new ProjectCardDto(
                Id: p.Id,
                Name: p.Name,
                UsersCount: p.Members.Count,
                CreatorNickName: p.Creator.NickName ?? string.Empty,
                CurrentUserRole: _dbcontext.ProjectMembers 
                    .FirstOrDefault(u => u.Id == userId).Role
            ))
            .ToList();
            */

        return projects;
    }

    public async Task UpdateProject(Guid projectId, ProjectDto projectDto)
    {
        var p = await _dbcontext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ?? throw new NotFoundException();
        
        p.Name = projectDto.Name;
        p.DeadLine = projectDto.DeadLine ?? p.DeadLine;
        p.CreatorId = projectDto.CreatorId;
        p.RootTask = projectDto.RootTask;
        p.Members = projectDto.Members;
        p.Tasks =  projectDto.Tasks;

        await _dbcontext.SaveChangesAsync();
    }

    public async Task DeleteProject(Guid projectId, Guid userId)
    {
        var project = await _dbcontext.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId)
            ??  throw new NotFoundException();

        _dbcontext.Remove(project);
        await _dbcontext.SaveChangesAsync();
    }
    
    public async Task<ProjectOnLoadDto> GetFullProjectInfo 
        (Guid projectId, Guid userId)
    {
        var project =  await _dbcontext.Projects
                           .AsNoTracking()
                           .Include(p => p.Members)
                           .ThenInclude(m => m.User)             
                           .Include(p => p.Teams)
                           .Include(p => p.Tasks)
                           .ThenInclude(t => t.Dependencies)
                           .Include(p => p.Tasks)
                           .ThenInclude(t => t.Performers)   
                           .Include(p => p.InviteCodes)
                           .Include(p => p.Creator)
                           .Include(p => p.RootTask)
                           .FirstOrDefaultAsync(p=> p.Id == projectId)
                       ?? throw new NotFoundException();
        
        var projectInfo = new ProjectOnLoadDto
        (
                project.Id,
                project.Name,
                project.Creator,
                project.DeadLine,
                project.RootTask,
                project.Tasks,
                project.Members,
                project.Teams,
                project.InviteCodes.Select(InviteCode.ToDto).ToList() ?? new ()
        );
        return projectInfo;
    }

    public async Task SetProjectRootTask(Guid projectId, Guid taskId)
    {
        var task = _dbcontext.Tasks
            .FirstOrDefault(t => t.Id == taskId)
            ?? throw new NotFoundException();
        
        var project = _dbcontext.Projects
            .FirstOrDefault(p => p.Id == projectId)
            ?? throw new NotFoundException();
        
        project.RootTaskId =  taskId;
        project.RootTask = task;

        await _dbcontext.SaveChangesAsync();
    }
}