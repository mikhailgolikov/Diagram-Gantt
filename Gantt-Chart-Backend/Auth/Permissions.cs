namespace Gantt_Chart_Backend.Auth;

public static class Permissions
{
    public const string CreateProject = nameof(CreateProject);
    
    
    public const string CreateTask = nameof(CreateTask);
    public const string ReadTask   = nameof(ReadTask);
    public const string UpdateTask = nameof(UpdateTask);
    public const string DeleteTask = nameof(DeleteTask);
    
    public const string SetPerformer     = nameof(SetPerformer);
    public const string SetTaskDuration  = nameof(SetTaskDuration);
    public const string SetDependence    = nameof(SetDependence);
    public const string AddComment       = nameof(AddComment);
    public const string RemoveComment    = nameof(RemoveComment);
    public const string UpdateTaskStatus = nameof(UpdateTaskStatus);
    
    public const string CreateTeam    = nameof(CreateTeam);
    public const string ReadTeam      = nameof(ReadTeam);
    public const string UpdateTeam    = nameof(UpdateTeam);
    public const string DeleteTeam    = nameof(DeleteTeam);
    public const string DeleteProject = nameof(DeleteProject);
    
    
    public const string UpdateProject      = nameof(UpdateProject);
    public const string SetRootTask        = nameof(SetRootTask);
    public const string SetProjectDuration = nameof(SetProjectDuration);
    public const string AddUser            = nameof(AddUser);
    public const string RemoveUser         = nameof(RemoveUser);
    public const string SetUserRole        = nameof(SetUserRole);
    
}