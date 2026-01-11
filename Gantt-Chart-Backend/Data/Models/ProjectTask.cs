
namespace Gantt_Chart_Backend.Data.Models;

public class ProjectTask
{
    public ProjectTask()
    {
        Performers = new List<ProjectMember>();
        Teams = new List<Team>();
        Dependencies = new List<Dependence>();
        Comments = new List<Comment>();
    }
    
    public ProjectTask(
        string name,
        Guid projectId,
        DateTime? startTime,
        DateTime? endTime,
        string description = ""
        ) :  this()
    {
        Id = Guid.NewGuid();
        Name = name;
        ProjectId = projectId;
        Description = description;
        IsCompleted = false;
        StartTime = startTime ?? DateTime.Now;
        EndTime = endTime ??  DateTime.Now.AddDays(1);
    }
    
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public bool IsCompleted { get; set; }
    public Guid ProjectId { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public List<ProjectMember> Performers { get; set; }
    public List<Team> Teams { get; set; }
    public List<Dependence> Dependencies { get; set; }
    public List<Comment> Comments { get; set; }
    public Project Project { get; set; }

    public bool CanBeCompleted()
    {
        if (Dependencies.Count == 0) 
            return true;
        
        return Dependencies.All(d => d.Completed());
    }
}

