using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Services.Interfaces;

public interface ITaskService
{
    public Task<ProjectTask> GetTask(Guid taskId);
    public Task<Guid> AddTask(ProjectTaskDto task);
    public Task UpdateTask(ProjectTaskDto taskDto, Guid taskId);
    public Task DeleteTask(Guid taskId);
    public Task AddTaskDependence(DependenceDto depDto);
    public Task RemoveTaskDependence(DependenceDto depDto);
    public Task AddTaskPerformer(Guid taskId, Guid userId, int n);
    public Task RemoveTaskPerformer(Guid taskId, Guid userId, int n);
    public Task<(string, bool)> SetTaskStatus (Guid taskId, bool status);
    public Task<Guid> AddTaskComment (CommentDto commentDto);
    public Task RemoveTaskComment (Guid taskId, Guid commentId);
    
}