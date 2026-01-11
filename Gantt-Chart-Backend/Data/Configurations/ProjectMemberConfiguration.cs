using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gantt_Chart_Backend.Data.Configurations;

public class ProjectMemberConfiguration : IEntityTypeConfiguration<ProjectMember>
{
    public void Configure(EntityTypeBuilder<ProjectMember> builder)
    {
        builder.ToTable("project_member");

        builder.HasKey(pm => new { pm.Id, pm.ProjectId });

        builder.HasMany(pm => pm.Permissions).WithMany();
        
        builder.Property(pm => pm.Id)
            .IsRequired();

        builder.Property(pm => pm.ProjectId)
            .IsRequired();

        builder.Property(pm => pm.Role)
            .HasConversion(
                r => r.ToString(),
                r => (Role)Enum.Parse(typeof(Role), r))
            .IsRequired();
            
        builder.HasOne(pm => pm.User)
            .WithMany(u => u.Roles)
            .HasForeignKey(pm => pm.Id)
            .OnDelete(DeleteBehavior.Cascade);
    }
}