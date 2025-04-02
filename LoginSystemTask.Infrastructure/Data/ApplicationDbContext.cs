using LoginSystemTask.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystemTask.Infrastructure.Data
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Permission> Permissions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }
        public DbSet<RolePermission> RolePermissions { get; set; }
        public DbSet<SalaryDetails> SalaryDetails { get; set; } 

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.EmployeeCode)
                .IsUnique();
            modelBuilder.Entity<Employee>()
            .Property(e => e.Salary)
            .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<User>()
            .HasOne(u => u.Employee)
            .WithOne(e => e.User)
            .HasForeignKey<Employee>(e => e.UserId)
            .IsRequired(false); 

            modelBuilder.Entity<Role>()
                .HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);

            modelBuilder.Entity<User>()
                .HasMany(u => u.AuditLogs)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
        
            modelBuilder.Entity<RolePermission>()
                .HasKey(rp => new { rp.RoleId, rp.PermissionId });

            modelBuilder.Entity<SalaryDetails>()
                .Property(sd => sd.Bonus)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<SalaryDetails>()
                .Property(sd => sd.Deductions)
                .HasColumnType("decimal(10,2)");

            modelBuilder.Entity<SalaryDetails>()
                .Property(sd => sd.Tax)
                .HasColumnType("decimal(10,2)");
            modelBuilder.Entity<Employee>()
            .HasOne(e => e.SalaryDetails)
            .WithOne(sd => sd.Employee)
            .HasForeignKey<SalaryDetails>(sd => sd.EmployeeId)
            .IsRequired(false);


            // Seed data
            modelBuilder.Entity<Role>().HasData(
            new Role { Id = 1, Name = "Admin" },
            new Role { Id = 2, Name = "User" });

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), 
                    RoleId = 1 
                });

            modelBuilder.Entity<Permission>().HasData(
                        new Permission { Id = 1, Name = "CanEditRoles" }, 
                        new Permission { Id = 2, Name = "CanDeleteEmployee" }
                    );

            modelBuilder.Entity<RolePermission>().HasData(
                new RolePermission { RoleId = 1, PermissionId = 1 }, 
                new RolePermission { RoleId = 1, PermissionId = 2 }  
            );
        }
    }
}
