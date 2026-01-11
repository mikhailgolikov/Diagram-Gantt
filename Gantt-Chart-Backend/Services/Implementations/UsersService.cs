using System.Security.Authentication;
using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.DTOs;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Exceptions;
using Gantt_Chart_Backend.Repositories.Interfaces;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Gantt_Chart_Backend.Services.Implementations;

public class UsersService : IUsersService
{
    private readonly IPasswordHasher _passwordHasher;
    private readonly GanttPlatformDbContext _dbcontext;
    private readonly IJwtProvider _jwtProvider;

    public UsersService (
        IPasswordHasher passwordHasher, 
        GanttPlatformDbContext dbcontext,
        IJwtProvider jwtProvider
        )
    {
        _passwordHasher = passwordHasher;
        _dbcontext = dbcontext;
        _jwtProvider = jwtProvider;
    }
    
    public async Task<Guid> Register (UserRequestDto userRequestDto)
    {
        var user = await _dbcontext.Users.FirstOrDefaultAsync(u => u.Email == userRequestDto.Email);

        if (user is not null)
            throw new UserAlreadyExistsException();
        
        var newUser = new User()
        {
            Id = Guid.NewGuid(),
            NickName = userRequestDto.NickName ?? "",
            Email = userRequestDto.Email,
            PasswordHash = _passwordHasher.GeneratePasswordHash(userRequestDto.Password)
        };
        
        _dbcontext.Users.Add(newUser);
        
        await _dbcontext.SaveChangesAsync();
        
        return newUser.Id;
    }

    public async Task<string> Login (LoginUserRequest userDto)
    {
        var user = await _dbcontext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Email == userDto.Email)
            ?? throw new NotFoundException();
        
        if (!_passwordHasher.VerifyPassword(userDto.Password, user.PasswordHash)) 
            throw new InvalidCredentialsException();
        
        var token = _jwtProvider.GenerateToken(user);

        return token;
    }

    public Task DeleteUser()
    {
        throw new NotImplementedException();
    }


    public async Task UpdateUserData(UpdateProfileDto userDto, Guid userId)
    {
        if (userDto == null)
            throw new ArgumentNullException();
        
        var user = await _dbcontext.Users
            .FirstOrDefaultAsync(u => u.Id == userId)
            ?? throw new NotFoundException();
        
        if (userDto.CurrentPassword is null || 
            !_passwordHasher.VerifyPassword(userDto.CurrentPassword, user.PasswordHash)) 
            throw new InvalidCredentialsException();
        
        user.Email = userDto.Email ?? user.Email;
        user.NickName = userDto.NickName ?? user.NickName;

        try
        {
            await _dbcontext.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            throw new ConflictException("Email already in use");
        }
    }

    public async Task UpdateUserPassword(UpdateProfileDto userRequestDto, Guid userId)
    {
        var user = await _dbcontext.Users
                   .FirstOrDefaultAsync(u => u.Id == userId)
                   ?? throw new NotFoundException("User not found");
        
        if (_passwordHasher.VerifyPassword(userRequestDto.CurrentPassword, user.PasswordHash)
            && userRequestDto.NewPassword is not null)
            user.PasswordHash = _passwordHasher.GeneratePasswordHash(userRequestDto.NewPassword);
        else 
            throw new InvalidCredentialException();
        
        await _dbcontext.SaveChangesAsync();
    }

    public async Task<ICollection<Permission>> GetUserPermissionsByName(string name)
    {
        return await _dbcontext.ProjectMembers
            .AsNoTracking()
            //.Include(p => p.User)
            //.Include(p => p.Permissions)
            .Where(u => u.User.NickName.Contains(name))
            .SelectMany(pm => pm.Permissions)
            .Distinct()
            .ToListAsync();
    }
}