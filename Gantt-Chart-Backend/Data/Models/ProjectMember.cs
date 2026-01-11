using Gantt_Chart_Backend.Data.Enums;

namespace Gantt_Chart_Backend.Data.Models;

public class ProjectMember
{
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    
    public Role Role { get; set; }
    
    public User User { get; set; }
    public Project Project { get; set; }

    public ICollection<Permission> Permissions { get; set; } = new List<Permission>();
}
