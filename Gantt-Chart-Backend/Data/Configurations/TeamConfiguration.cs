using Gantt_Chart_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gantt_Chart_Backend.Data.Configurations;

public class TeamConfiguration : IEntityTypeConfiguration<Team>
{
    public void Configure(EntityTypeBuilder<Team> builder)
    {
        builder.ToTable("team");

        builder.HasKey(t => t.Id);
        
        builder.Property(t => t.LeaderId)
            .IsRequired();
            
        builder.HasOne(t => t.Leader)
            .WithMany() 
            .HasForeignKey(t => new { t.LeaderId, t.ProjectId })
            .HasPrincipalKey(pm => new { pm.Id, pm.ProjectId }) 
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.Performers)
            .WithMany();
        
        builder
            .Property(t => t.ProjectId)
            .IsRequired();
    }
}
