namespace Gantt_Chart_Backend.Data.Models;

public class User
{
    public Guid Id { get; set; }
    public string NickName { get; set; }
    public string Email { get; set; }
    public string PasswordHash { get; set; }

    public ICollection<ProjectMember> Roles { get; set; } = new List<ProjectMember>();
}