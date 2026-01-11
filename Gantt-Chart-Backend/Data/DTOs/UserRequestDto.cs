namespace Gantt_Chart_Backend.Data.DTOs;

public record UserRequestDto (
    string NickName,
    string Email,
    string Password
    );