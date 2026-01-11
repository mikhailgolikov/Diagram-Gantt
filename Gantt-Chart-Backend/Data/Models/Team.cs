
namespace Gantt_Chart_Backend.Data.Models;

public class Team
{
    public Team()
    {
        Performers = new List<ProjectMember>();
    }
    
    public Team (
        string name,
        Guid leaderId,
        Guid projectId
    ) : this()
    {
        Id = Guid.NewGuid();
        Name = name;
        LeaderId = leaderId;
        ProjectId = projectId;
    }
    
    public string Name { get; set; }
    public Guid Id { get; set; }
    public Guid LeaderId { get; set; }
    public Guid ProjectId { get; set; }
    public List<ProjectMember> Performers { get; set; }
    public ProjectMember Leader { get; set; }
    public Project Project { get; set; }
}