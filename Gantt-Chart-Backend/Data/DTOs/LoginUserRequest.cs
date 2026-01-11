using System.ComponentModel.DataAnnotations;

namespace Gantt_Chart_Backend.Data.DTOs;

public record LoginUserRequest(
    [Required]  string Email,
    [Required]  string Password
    );