using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;

namespace Gantt_Chart_Backend.Data.Models;

public class Dependence
{
    public Guid Id { get; set; }
    public DependencyType Type { get; set; }
    public Guid ChildId { get; set; }
    public Guid ParentId { get; set; }
    
    public ProjectTask ChildTask { get; set; }
    public ProjectTask ParentTask { get; set; }

    public static Dependence FromDto(DependenceDto dto)
    {
        return new Dependence
        {
            Id = Guid.NewGuid(),
            ChildId = dto.ChildId,
            ParentId = dto.ParentId,
            Type = dto.Type
        };
    }

    public bool Completed()
    {
        return Type switch
        {
            // FS: Parent не может начаться, пока не закончится Child
            DependencyType.FS => ParentTask.StartTime >= ChildTask.EndTime
                                 && ChildTask.IsCompleted,
            
            // SS: Parent не может начаться раньше Child
            DependencyType.SS => ParentTask.StartTime >= ChildTask.StartTime,
            
            // FF: Parent не может завершиться, пока не завершится Child
            DependencyType.FF => ParentTask.EndTime >= ChildTask.EndTime 
                                 && ChildTask.IsCompleted,
            
            // SF: Parent не может завершиться, пока не начнётся Child
            DependencyType.SF => ParentTask.EndTime >= ChildTask.StartTime,
            
            _ => false
        };
    }
}