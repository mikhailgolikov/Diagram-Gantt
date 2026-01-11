using System.Text;
using Gantt_Chart_Backend.Auth;
using Gantt_Chart_Backend.Data.Enums;
using Gantt_Chart_Backend.Data.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Gantt_Chart_Backend.Extensions;

public static class ApiExtensions
{
    public static void AddApiAuthentication(
        this IServiceCollection services,
        IOptions<JwtOptions> jwtOptions)
    {
        services.AddSingleton<IAuthorizationHandler, PermissionRequirementsHandler>();
        services
            .AddAuthorization(options =>
            {
                options.AddPolicy(Permissions.CreateTask, builder =>
                {
                    builder.Requirements.Add(new PermissionRequirements(Permissions.CreateTask));
                });
            })
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, options =>
            {
                options.TokenValidationParameters = new()
                {
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwtOptions.Value.SecretKey))
                };
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["jwt-token"];
                        return Task.CompletedTask;
                    }
                };
            });

        //services.AddScoped<IPermissionService, PermissionService>();
        
        /*services.AddAuthorization(options =>
        {
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireClaim("Role", "Admin");
                

            });
            
            options.AddPolicy("AdminPolicy", policy =>
            {
                policy.RequireClaim("Role", "Admin");
                

            });
        });*/
    }
}