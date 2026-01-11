using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gantt_Chart_Backend.Services.Implementations;

public class TaskService : ITaskService
{
    private readonly GanttPlatformDbContext _dbcontext;

    public TaskService(GanttPlatformDbContext dbContext)
    {
        _dbcontext = dbContext;
    }
    
    public async Task<ProjectTask> GetTask(Guid taskId)
    {
        return await _dbcontext.Tasks
            .Include(t => t.Performers)
            //.Include(t => t.Teams)
            .Include(t => t.Dependencies)
            .Include(t => t.Comments)
            .FirstOrDefaultAsync(t => t.Id == taskId) 
               ?? throw new NotFoundException();
    }

    public async Task<Guid> AddTask(ProjectTaskDto task)
    {
        var newTask = new ProjectTask(
            task.Name ?? throw new ArgumentException(),
            task.ProjectId,
            task.StartTime,
            task.EndTime,
            task.Description
        );
        
        /*Description = task.Description ?? "",
            IsCompleted = task.IsCompleted ?? false,
            Dependencies = task.Dependencies ?? new List<Dependence>(),
            StartTime = task.StartTime ??  DateTime.UtcNow,
            EndTime = task.EndTime ??  DateTime.UtcNow.AddDays(1),*/
        
        await _dbcontext.Tasks.AddAsync(newTask);
        
        await _dbcontext.SaveChangesAsync();
        
        return newTask.Id;
    }
    
    public async Task UpdateTask(ProjectTaskDto taskDto, Guid taskId)
    {
        var task = await _dbcontext.Tasks
                       .Include(projectTask => projectTask.Dependencies)
                       .FirstOrDefaultAsync(t => t.Id == taskId)
                   ?? throw new NotFoundException();
        
        task.Name = taskDto.Name ?? task.Name;
        task.Description = taskDto.Description ?? task.Description;
        task.IsCompleted = taskDto.IsCompleted ?? task.IsCompleted;
        task.Dependencies = taskDto.Dependencies ?? task.Dependencies;
        task.StartTime = taskDto.StartTime ?? task.StartTime;
        task.EndTime = taskDto.EndTime ?? task.EndTime;

        await _dbcontext.SaveChangesAsync();
    }

    public async Task DeleteTask(Guid taskId)
    {
         _dbcontext.Tasks
             .Remove(
                 _dbcontext.Tasks
                     .FirstOrDefault(t => t.Id == taskId) 
                 ?? throw new NotFoundException());
         
         await _dbcontext.SaveChangesAsync();
    }
    
    public async Task AddTaskDependence(DependenceDto depDto)
    {
        if (depDto.ChildId == depDto.ParentId)
            throw new InvalidOperationException();

        var parentExists = await _dbcontext.Tasks
            .FirstOrDefaultAsync(t => t.Id == depDto.ParentId);
        
        var childExists = await _dbcontext.Tasks
            .FirstOrDefaultAsync(t => t.Id == depDto.ChildId);
    
        if (parentExists is null || childExists is null)
            throw new NotFoundException();

        var exists = await _dbcontext.Dependences
            .FirstOrDefaultAsync(t => t.ParentId == depDto.ParentId
                                      && t.ChildId == depDto.ChildId);
        
        if (exists != null) 
            throw new DependenceAlreadyExistsException();
        
        _dbcontext.Dependences.Add(Dependence.FromDto(depDto));
        
        await _dbcontext.SaveChangesAsync();
    }

    public async Task RemoveTaskDependence(DependenceDto depDto)
    {
        var dep = await _dbcontext.Dependences
            .FirstOrDefaultAsync(d => d.ParentId == depDto.ParentId 
                                      && d.ChildId == depDto.ChildId)
            ?? throw new NotFoundException();
        
        _dbcontext.Dependences.Remove(dep);
        
        await _dbcontext.SaveChangesAsync();
    }

    public Task AddTaskPerformer(Guid taskId, Guid userId, int n)
    {
        var task = _dbcontext.Tasks
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException();
        
        if (n == 0)
        {
            var performer = _dbcontext.ProjectMembers.FirstOrDefault(u => u.Id == userId) ?? throw new NotFoundException();
            task.Performers.Add(performer);
        }
        else
        {
            var team = _dbcontext.Teams.FirstOrDefault(u => u.Id == userId) ?? throw new NotFoundException();
            task.Teams.Add(team);
        }
        _dbcontext.SaveChanges();
        
        return Task.CompletedTask;
    }

    public async Task RemoveTaskPerformer(Guid taskId, Guid userId, int n)
    {
        var task = _dbcontext.Tasks
            .Include(t => t.Performers)
            .Include(t => t.Teams)
            .FirstOrDefault(t => t.Id == taskId) ?? throw new NotFoundException();
        
        if (n == 0)
        {
            var performer = _dbcontext.ProjectMembers
                .FirstOrDefault(u => u.Id == userId) 
                            ?? throw new NotFoundException();
            task.Performers.Remove(performer);
        }
        else
        {
            var team = _dbcontext.Teams
                .FirstOrDefault(u => u.Id == userId) 
                       ?? throw new NotFoundException();
            task.Teams.Remove(team);
        }
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<(string, bool)> SetTaskStatus(Guid taskId, bool status)
    {
        var task = await _dbcontext.Tasks.FirstOrDefaultAsync(t => t.Id == taskId)
            ?? throw new NotFoundException();
        
        if (!task.CanBeCompleted())
            return ("Task can't be completed", false);

        task.IsCompleted = status;
        
        await _dbcontext.SaveChangesAsync();
        
        return ("Task has been completed", true);
    }

    public async Task<Guid> AddTaskComment(CommentDto commentDto)
    {
        var task = await _dbcontext.Tasks
            .Include(task => task.Comments)
            .FirstOrDefaultAsync(t => t.Id == commentDto.TaskId) 
                   ?? throw new NotFoundException();
                   
    
        var newComment = new Comment (
            commentDto.TaskId,
            commentDto.AuthorId,
            commentDto.Content,
            commentDto.CreatedAt ?? DateTime.UtcNow
            );
        
        
        _dbcontext.Comments.Add(newComment);
        
        await _dbcontext.SaveChangesAsync();
        
        return newComment.Id;
    }

    public async Task RemoveTaskComment(Guid taskId, Guid commentId)
    {
        var comment = await _dbcontext.Comments
            .FirstOrDefaultAsync(c => c.Id == commentId && c.TaskId == taskId) 
                      ?? throw new NotFoundException();
        
        _dbcontext.Comments.Remove(comment);
        
        await _dbcontext.SaveChangesAsync();
    }
}