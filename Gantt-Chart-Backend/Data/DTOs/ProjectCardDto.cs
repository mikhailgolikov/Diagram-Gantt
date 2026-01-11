using Gantt_Chart_Backend.Data.Enums;

namespace Gantt_Chart_Backend.Data.DTOs;

public record ProjectCardDto (
    Guid Id, 
    string Name,
    int UsersCount,
    string CreatorNickName,
    Role CurrentUserRole
    );
    