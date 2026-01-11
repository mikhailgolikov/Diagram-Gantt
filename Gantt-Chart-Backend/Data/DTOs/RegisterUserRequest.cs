using System.ComponentModel.DataAnnotations;

namespace Gantt_Chart_Backend.Data.DTOs;

public record RegisterUserRequest (
    [Required] string Email,
    [Required] string Password,
    [Required] string Username
    );