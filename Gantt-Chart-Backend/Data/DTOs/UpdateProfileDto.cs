namespace Gantt_Chart_Backend.Data.DTOs;

public class UpdateProfileDto
{
    public string? Email { get; set; }
    public string? NickName { get; set; }
    public string CurrentPassword { get; set; }
    public string? NewPassword { get; set; }
}