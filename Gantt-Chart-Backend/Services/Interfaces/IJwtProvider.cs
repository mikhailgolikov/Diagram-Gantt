using Gantt_Chart_Backend.Data.Models;

namespace Gantt_Chart_Backend.Services.Interfaces;

public interface IJwtProvider
{
    public string GenerateToken(User user);
}