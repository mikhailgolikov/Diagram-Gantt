namespace Gantt_Chart_Backend.Data.DTOs;

public record CommentDto(
    Guid TaskId,
    string Content,
    Guid AuthorId,
    DateTime? CreatedAt
    );
