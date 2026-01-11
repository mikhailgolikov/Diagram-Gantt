namespace Gantt_Chart_Backend.Services.Interfaces;

public interface IPasswordHasher
{
    public string GeneratePasswordHash(string password);

    public bool VerifyPassword(string password, string hashedPassword);
}