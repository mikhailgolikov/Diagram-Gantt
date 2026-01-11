using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Data.DTOs;

public record ProjectTaskDto(
    Guid ProjectId,
    string Name,
    string? Description,
    bool? IsCompleted,
    List<Dependence>?  Dependencies,
    DateTime? StartTime = null,
    DateTime? EndTime = null
    );