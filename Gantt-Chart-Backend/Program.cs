using System.Text.Json.Serialization;
using System.Text.Json.Serialization.Metadata;
using Gantt_Chart_Backend.Data.DbContext;
using Gantt_Chart_Backend.Data.Models;
using Gantt_Chart_Backend.Extensions;
using Gantt_Chart_Backend.Services.Implementations;
using Gantt_Chart_Backend.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);


builder.Services.Configure<JwtOptions>
    (builder.Configuration.GetSection(nameof(JwtOptions)));


var isRunningInContainer = Environment.GetEnvironmentVariable("DOTNET_RUNNING_IN_CONTAINER") == "true";
var dbName = Environment.GetEnvironmentVariable("DB_NAME") ?? "GanttDb";
var dbUser = Environment.GetEnvironmentVariable("DB_USER") ?? "postgres";
var dbPassword = Environment.GetEnvironmentVariable("DB_PASSWORD") ?? "password";
var dbHost = isRunningInContainer 
    ? "postgres"
    : "localhost";

var connectionString = $"Host={dbHost};Port=5432;Database={dbName};Username={dbUser};Password={dbPassword}";


builder.Services.AddDbContext<GanttPlatformDbContext>(options => 
    options.UseNpgsql(
        connectionString
        //builder.Configuration.GetConnectionString("DefaultConnection")
        ));

builder.Services
    .AddControllers()
    .AddJsonOptions(options => 
        options.JsonSerializerOptions.ReferenceHandler = 
            ReferenceHandler.IgnoreCycles);


builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUsersService, UsersService>();
builder.Services.AddScoped<IProjectService, ProjectService>();
builder.Services.AddScoped<IAuthenticationService, AuthenticationService>();
builder.Services.AddScoped<ITaskService, TaskService>();
builder.Services.AddScoped<ITeamService, TeamService>();
builder.Services.AddScoped<IInviteService, InviteService>();


builder.Services.AddScoped<IPasswordHasher,  PasswordHasher>();
builder.Services.AddScoped<IJwtProvider, JwtProvider>();

var corsOrigins  = Environment.GetEnvironmentVariable("ASPNETCORE_CORS_ORIGINS");
var origins = new List<string>
{
    "http://localhost:5173",
    "http://localhost:4173",
    "http://frontend:4173",
    "http://frontend:80",
    "http://localhost:3000",
    "https://yourdomain.com",
    "http://141.98.189.35",
    "https://141.98.189.35"
};

if (!string.IsNullOrEmpty(corsOrigins))
{
    origins.AddRange(
        corsOrigins
            .Split(',', StringSplitOptions.RemoveEmptyEntries)
            .Select(o => o.Trim())
            .Where(o => !string.IsNullOrEmpty(o))
        );
}

Console.WriteLine($"CORS ORIGINS: {string.Join(", ", origins)}");

builder.Services.AddCors(options => {
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            //.AllowAnyOrigin()
            .WithOrigins(origins.ToArray())
            .AllowAnyHeader()
            .AllowAnyMethod()
        .AllowCredentials();
    });
});

builder.Services.AddApiAuthentication(
    builder.Services.BuildServiceProvider().GetRequiredService<IOptions<JwtOptions>>());

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRouting();

app.UseCors("AllowAll");


app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var dbContext =  scope.ServiceProvider.GetRequiredService<GanttPlatformDbContext>();
    await dbContext.Database.MigrateAsync();
}

app.Run();