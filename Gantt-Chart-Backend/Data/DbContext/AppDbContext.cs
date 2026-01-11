using Gantt_Chart_Backend.Data.Configurations;
using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Data.DbContext;
using Microsoft.EntityFrameworkCore;

public class GanttPlatformDbContext(DbContextOptions<GanttPlatformDbContext> options) 
    :  DbContext(options)
{
    public DbSet<User> Users { get; set; }
    public DbSet<Project> Projects { get; set; }
    public DbSet<ProjectMember> ProjectMembers { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<ProjectTask> Tasks { get; set; }
    public DbSet<Comment> Comments { get; set; }
    public DbSet<Dependence> Dependences { get; set; }
    public DbSet<InviteCode> InviteCodes { get; set; }
    

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserConfiguration          ());
        modelBuilder.ApplyConfiguration(new ProjectConfiguration       ());
        modelBuilder.ApplyConfiguration(new ProjectMemberConfiguration ());
        modelBuilder.ApplyConfiguration(new TeamConfiguration          ());
        modelBuilder.ApplyConfiguration(new TaskConfiguration          ());
        modelBuilder.ApplyConfiguration(new DependenceConfiguration    ());
        modelBuilder.ApplyConfiguration(new PermissionConfiguration    ());
        modelBuilder.ApplyConfiguration(new InviteCodeConfiguration    ());
        
    }
}

