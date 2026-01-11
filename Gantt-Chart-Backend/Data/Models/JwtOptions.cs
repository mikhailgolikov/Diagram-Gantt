namespace Gantt_Chart_Backend.Data.Models;

public class JwtOptions
{
    public string SecretKey { get; set; } = string.Empty;
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpireHours { get; set; } = 84;
}