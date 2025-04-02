using LoginSystemTask.Application.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Data;
using LoginSystemTask.Infrastructure.Interfaces;
using LoginSystemTask.Infrastructure.Interfaces.IRepositories;
using LoginSystemTask.Infrastructure.Repositories;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace LoginSystemTask.Infrastructure
{
    public static class InfrastructureRegistration
    {
        public static void AddInfrastructureRegistration(this WebApplicationBuilder builder)
        {

            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                    });
            
            builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddScoped<IEmployeeRepository, EmployeeRepository>();
            builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();
            builder.Services.AddScoped<IRoleRepository, RoleRepository>();
        }
    }
}
