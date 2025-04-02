using LoginSystemTask.Application.Helpers;
using LoginSystemTask.Application.Interfaces.IServices;
using LoginSystemTask.Application.Interfaces.IServices.Authentication;
using LoginSystemTask.Application.Services;
using LoginSystemTask.Application.Services.Authentication;
using LoginSystemTask.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace LoginSystemTask.Application
{
    public static class ApplicationRegistration
    {
        public static void AddApplicationRegistration(this WebApplicationBuilder builder)
        {

            // JWT Options Configuration
            builder.Services.Configure<JwtOptions>(builder.Configuration.GetSection("JWT"));

            // Authentication Setup
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(o =>
            {
                o.RequireHttpsMetadata = builder.Environment.IsDevelopment() ? false : true; 
                o.SaveToken = true; 
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero, 
                    ValidIssuer = builder.Configuration["JWT:Issuer"],
                    ValidAudience = builder.Configuration["JWT:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                };
            });

            builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("AdminOnly", policy => policy.RequireRole("Admin"));
            });

            //// Register Services
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IAuthService, AuthService>();
            builder.Services.AddScoped<IRoleService, RoleService>();
            builder.Services.AddScoped<IPermissionService, PermissionService>();
            builder.Services.AddScoped<IAuditLogService, AuditLogService>();
            builder.Services.AddScoped<IEmployeeService, EmployeeService>();
            builder.Services.AddScoped<IUserService, UserService>();
        }
    }
}