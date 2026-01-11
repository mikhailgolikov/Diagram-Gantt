namespace Gantt_Chart_Backend.Services.Interfaces;

public interface IInviteService
{
    public string GenerateCode();
    public Task AddUserToProjectByInviteCode(Guid userId, string code);
}