using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gantt_Chart_Backend.Data.Configurations;

public class DependenceConfiguration : IEntityTypeConfiguration<Dependence>
{
    public void Configure(EntityTypeBuilder<Dependence> builder)
    {
        builder.ToTable("dependence");

        builder.HasKey(d => d.Id);

        builder.Property(d => d.Type)
            .HasConversion(
                r => r.ToString(),
                r => (DependencyType)Enum.Parse(typeof(DependencyType), r))
            .IsRequired();

        builder.Property(d => d.ChildId)
            .IsRequired();

        builder.Property(d => d.ParentId)
            .IsRequired();
            
        builder.HasOne(d => d.ChildTask)
            .WithMany(t => t.Dependencies)  
            .HasForeignKey(d => d.ChildId)
            .OnDelete(DeleteBehavior.Cascade);

           
        builder.HasOne(d => d.ParentTask)
            .WithMany()                     
            .HasForeignKey(d => d.ParentId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
