using Gantt_Chart_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gantt_Chart_Backend.Data.Configurations;

public class TaskConfiguration : IEntityTypeConfiguration<ProjectTask>
{
    public void Configure(EntityTypeBuilder<ProjectTask> builder)
    {
        builder.ToTable("task");
    
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired();

        builder.Property(t => t.Description);

        builder.HasOne(t => t.Project)
            .WithMany(p => p.Tasks)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(t => t.Performers)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "task_performers_users",
                j => j.HasOne<ProjectMember>()
                    .WithMany()
                    .HasForeignKey("project_member_id", "project_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<ProjectTask>()
                    .WithMany()
                    .HasForeignKey("task_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("task_id", "project_member_id", "project_id");
                    j.ToTable("task_performers");
                });
        
        builder.HasMany(t => t.Teams)
            .WithMany()
            .UsingEntity<Dictionary<string, object>>(
                "task_performers_teams",
                j => j.HasOne<Team>()
                    .WithMany()
                    .HasForeignKey("team_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<ProjectTask>()
                    .WithMany()
                    .HasForeignKey("task_id")
                    .OnDelete(DeleteBehavior.Cascade),
                j =>
                {
                    j.HasKey("task_id", "team_id");
                    j.ToTable("task_performers_teams");
                });
        
        builder.Property(t => t.StartTime)
            .IsRequired();

        builder.Property(t => t.EndTime)
            .IsRequired();
        
        builder.HasMany(t => t.Comments)
            .WithOne(c => c.Task)
            .HasForeignKey(c => c.TaskId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}