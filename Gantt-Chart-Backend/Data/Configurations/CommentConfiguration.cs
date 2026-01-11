using Gantt_Chart_Backend.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Gantt_Chart_Backend.Data.Configurations;

public class CommentConfiguration : IEntityTypeConfiguration<Comment>
{
    public void Configure(EntityTypeBuilder<Comment> builder)
    {
        builder.ToTable("comment");

        builder.HasKey(c => c.Id);
            
        builder.HasOne(c => c.Author)
            .WithMany()                       
            .HasForeignKey(c => c.AuthorId)
            .OnDelete(DeleteBehavior.Cascade);


        builder.HasOne(s => s.Task)
            .WithMany(t => t.Comments)
            .HasForeignKey(c => c.TaskId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}
