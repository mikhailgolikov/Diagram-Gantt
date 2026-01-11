using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Data.DTOs;

public record ProjectDto(
    Guid? Id,
    string Name,
    Guid CreatorId, 
    DateTime? DeadLine, 
    ProjectTask? RootTask, 
    List<ProjectTask>? Tasks, 
    List<ProjectMember>? Members, 
    List<Team>? Teams 
);