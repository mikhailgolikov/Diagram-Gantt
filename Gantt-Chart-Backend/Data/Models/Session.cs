namespace Gantt_Chart_Backend.Data.Models;

public class Session
{
    public Guid Id { get; set; }
    public Guid Token { get; set; }
    public Guid UserId { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public User User { get; set; }
}
