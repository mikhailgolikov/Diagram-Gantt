using Gantt_Chart_Backend.Data.DTOs;

namespace Gantt_Chart_Backend.Data.Models;

public class InviteCode
{
    public InviteCode(
        Guid projectId,
        string code
        )
    {
        Id = Guid.NewGuid();
        ProjectId = projectId;
        Code = code;
    }
    public Guid Id { get; set; }
    public Guid ProjectId { get; set; }
    public string Code { get; set; }
    
    /*
    public int InvitationsLeft { get; set; }
    public DateTime ExpiresAt { get; set; }
    public bool IsActive { get; set; }
    */
    
    public Project Project { get; set; }

    public static InviteCodeResponseDto ToDto(InviteCode inviteCode)
    {
        return new InviteCodeResponseDto(inviteCode.ProjectId, inviteCode.Code);
    }
}