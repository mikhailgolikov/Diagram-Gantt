using Gantt_Chart_Backend.Data.Enums;

namespace Gantt_Chart_Backend.Data.DTOs;

public record DependenceDto(
    Guid ParentId,
    Guid ChildId,
    DependencyType Type);