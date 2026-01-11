
using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Data.DTOs;

public record ProjectOnLoadDto
(
    Guid Id,
    string Name,
    User Creator,
    DateTime? DeadLine,
    ProjectTask RootTask,
    List<ProjectTask> Tasks,
    List<ProjectMember> Members,
    List<Team> Teams,
    List<InviteCodeResponseDto> InviteCodes
);