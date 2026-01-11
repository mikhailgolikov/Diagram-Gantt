namespace Gantt_Chart_Backend.Data.Models;

public class Permission (string name)
{
    public Guid Id { get; set; }
    public string Name { get; set; } = name;
}