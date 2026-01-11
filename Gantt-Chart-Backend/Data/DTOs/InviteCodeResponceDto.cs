namespace Gantt_Chart_Backend.Data.DTOs;

public record InviteCodeResponseDto(
    Guid ProjectId, 
    string Code
    );