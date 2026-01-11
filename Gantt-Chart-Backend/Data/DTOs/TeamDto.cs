using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Data.DTOs;

public record TeamDto(
    string Name,
    Guid ProjectId,
    Guid LeaderId,
    List<ProjectMember>? Members,
    Guid? Id
    );