namespace Gantt_Chart_Backend.Data.DTOs;

public record ProjectCreateDto(
    string Name,
    Guid CreatorId, 
    DateTime? DeadLine
    );