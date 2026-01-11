using System.Security.Cryptography;
using System.Text;
using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Services.Interfaces;

namespace Gantt_Chart_Backend.Services.Implementations;

public class InviteService : IInviteService
{
    private readonly GanttPlatformDbContext _context;
    private readonly ITeamService _teamService;

    private readonly char[] _inviteCodeSymbols = 
        "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
    private byte _codeLength = 6;
    
    public InviteService(
        GanttPlatformDbContext context,
        ITeamService teamService
        )
    {
        _context = context;
        _teamService =  teamService;
    }
    
    public string GenerateCode()
    {
        using var rng = RandomNumberGenerator.Create();
        
        byte[] bytes = new byte[_codeLength];
        rng.GetBytes(bytes);
        
        StringBuilder code = new StringBuilder(_codeLength);

        foreach (var value in bytes)
            code.Append(_inviteCodeSymbols[value % _inviteCodeSymbols.Length]);

        return code.ToString();
    }

    public async Task AddUserToProjectByInviteCode(
        Guid userId,
        string code)
    {
        var inviteCode = _context.InviteCodes
            .FirstOrDefault(c => c.Code == code)
            ?? throw new NotFoundException("Code not found");
        
        await _teamService.AddUserToProject(userId, inviteCode.ProjectId);
        await _teamService.SetUserRoleInProject(userId, inviteCode.ProjectId, Role.Member);
    }
}