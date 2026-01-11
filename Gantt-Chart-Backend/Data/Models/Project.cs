namespace Gantt_Chart_Backend.Data.Models;

public class Project
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public Guid CreatorId { get; set; }
    
    public Guid? RootTaskId { get; set; }
    public DateTime DeadLine { get; set; }
    public ProjectTask? RootTask { get; set; }
    public List<ProjectTask> Tasks { get; set; }
    public List<ProjectMember> Members { get; set; }
    public List<InviteCode> InviteCodes { get; set; }
    public List<Team> Teams { get; set; }
    
    public User Creator { get; set; }
}