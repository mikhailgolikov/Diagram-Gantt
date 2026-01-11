namespace Gantt_Chart_Backend.Data.Models;

public class Comment
{
    public Comment(
        Guid taskId,
        Guid authorId,
        string content,
        DateTime createdAt
        )
    {
        Id = Guid.NewGuid();
        TaskId = taskId;
        AuthorId = authorId;
        Content = content;
        CreatedAt = createdAt;
    }
    public Guid Id { get; set; }
    public Guid TaskId { get; set; }
    public Guid AuthorId { get; set; }
    public string Content { get; set; }
    public DateTime CreatedAt { get; set; }
    
    public ProjectTask Task { get; set; }
    /*public User Author { get; set; }
    ->*/
    public ProjectMember Author { get; set; }
}