using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Services.Interfaces;

public interface ITeamService {

    public Task<Guid> CreateTeam(TeamDto team);
    public Task AddTeamMember(Guid teamId, Guid memberId);
    public Task RemoveTeamMember(Guid  teamId, Guid memberId);
    
    public Task AddUserToProject(Guid userId, Guid projectId);
    public Task RemoveUserFromProject(Guid userId, Guid projectId);
    public Task SetUserRoleInProject(Guid userId, Guid projectId, Role roleId);
}