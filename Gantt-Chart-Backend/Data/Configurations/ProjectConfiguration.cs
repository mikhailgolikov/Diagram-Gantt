using Gantt_Chart_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gantt_Chart_Backend.Data.Configurations;

public class ProjectConfiguration : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable("project");

        builder.HasKey(p => p.Id);
       
        builder.Property(p => p.CreatorId)
            .IsRequired();

        
        builder.Property(p => p.RootTaskId)
            .IsRequired(false);
            
        
        
        builder.HasOne(p => p.Creator)
            .WithMany() 
            .HasForeignKey(p => p.CreatorId)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasOne(p => p.RootTask)
            .WithOne()
            .HasForeignKey<Project>(p => p.RootTaskId)
            .IsRequired(false)
            .OnDelete(DeleteBehavior.Cascade);
            
        builder.HasMany(p => p.Tasks)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
        
        builder.HasMany(p => p.Members)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.Teams)
            .WithOne(t => t.Project)
            .HasForeignKey(t => t.ProjectId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}